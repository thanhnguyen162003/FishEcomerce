using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetRegisterQuery;

public record GetRegisterByMonthQuery : IRequest<ResponseModel>
{
    public int Month { get; init; }
    public int Year { get; init; }
}

public class GetRegisterByMonthQueryHandler : IRequestHandler<GetRegisterByMonthQuery, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRegisterByMonthQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseModel> Handle(GetRegisterByMonthQuery request, CancellationToken cancellationToken)
    {
        var customers = await _unitOfWork.CustomerRepository.GetAll()
            .AsNoTracking()
            .Where(x => x.DeletedAt == null && x.CreatedAt.Value.Month == request.Month && x.CreatedAt.Value.Year == request.Year)
            .ToListAsync(cancellationToken);
        
        var mapper = _mapper.Map<List<CustomerResponseModel>>(customers);
        var count = mapper.Count;

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalCustomes = count,
            Customers = mapper
        });
    }
}