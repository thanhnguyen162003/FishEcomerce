using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetSalesQuery;

public record GetSalesByMonthQuery : IRequest<ResponseModel>
{
    public int Month { get; init; }
    public int Year { get; init; }
}

public class GetSalesByMonthQueryHandler : IRequestHandler<GetSalesByMonthQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesByMonthQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(GetSalesByMonthQuery request, CancellationToken cancellationToken)
    {
        var sales = await _unitOfWork.OrderRepository.GetAll()
            .Where(x => x.IsPaid == true && x.CreatedAt.Value.Month == request.Month && x.CreatedAt.Value.Year == request.Year)
            .GroupBy(x => (x.CreatedAt.Value.Day - 1) / 7 + 1)
            .Select(weekGroup => new
            {
                Week = weekGroup.Key,
                Sales = weekGroup.Sum(x => x.TotalPrice),
                Days = weekGroup.GroupBy(x => x.CreatedAt.Value.DayOfWeek).Select(x => new {
                    DayOfWeek = x.Key,
                    Sales = x.Sum(o => o.TotalPrice)
                })
                .OrderBy(dayGroup => dayGroup.DayOfWeek)
            })
            .OrderBy(group => group.Week)
            .ToListAsync(cancellationToken);

        var totalSales = sales.Sum(x => x.Sales);
        
        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalSales = totalSales,
            Sales = sales
        });
    }
}