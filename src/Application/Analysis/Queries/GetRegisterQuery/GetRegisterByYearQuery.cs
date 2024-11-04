using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetRegisterQuery;

public record GetRegisterByYearQuery : IRequest<ResponseModel>
{
    public int Year { get; init; }
}

public class GetRegisterByYearQueryHandler : IRequestHandler<GetRegisterByYearQuery, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRegisterByYearQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(GetRegisterByYearQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.CustomerRepository.GetAll().AsNoTracking()
            .Where(x => x.DeletedAt == null && x.RegistrationDate.Value.Year == request.Year)
            .GroupBy(x => x.RegistrationDate.Value.Month)
            .Select(group => new
            {
                Month = group.Key,
                CustomerCount = group.Count(),
                Customers = group.Select(customer => _mapper.Map<CustomerResponseModel>(customer)).ToList(),
            }).OrderBy(group => group.Month)
            .ToListAsync(cancellationToken);
        
        var count = customers.Sum(group => group.CustomerCount);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalCustomes = count,
            CustomersGroupByYear = customers
        });
    }
}