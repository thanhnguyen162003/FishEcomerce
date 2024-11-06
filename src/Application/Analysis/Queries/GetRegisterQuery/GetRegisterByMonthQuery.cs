using System.Globalization;
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
        var customers = await _unitOfWork.CustomerRepository.GetAll().AsNoTracking()
            .Where(x => x.DeletedAt == null && x.RegistrationDate.Value.Month == request.Month && x.RegistrationDate.Value.Year == request.Year)
            .GroupBy(x => (x.RegistrationDate.Value.Day - 1) / 7 + 1)
            .ToListAsync(cancellationToken);
        
        var groups = customers
            .Select(weekGroup => new
            {
                Week = weekGroup.Key,
                CustomersCount = weekGroup.Count(),
                Days = weekGroup.GroupBy(x => x.RegistrationDate.Value.DayOfWeek).Select(x => new
                    {
                        DayOfWeek = x.Key,
                        CustomersCount = x.Count(),
                        Customers = _mapper.Map<List<CustomerResponseModel>>(x.OrderBy(customer => customer.Name).ToList())
                    })
                    .OrderBy(dayGroup => dayGroup.DayOfWeek)
            })
            .OrderBy(group => group.Week)
            .ToList();
        
        var totalCustomers = groups.Sum(customer => customer.CustomersCount);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalCustomes = totalCustomers,
            CustomersGroupByWeek = groups
        });
    }
}