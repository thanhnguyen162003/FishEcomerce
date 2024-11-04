using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetSalesQuery;

public record GetSalesByYearQuery : IRequest<ResponseModel>
{
    public int Year { get; init; }
}

public class GetSalesByYearQueryHandler : IRequestHandler<GetSalesByYearQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesByYearQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ResponseModel> Handle(GetSalesByYearQuery request, CancellationToken cancellationToken)
    {
        var sales = await _unitOfWork.OrderRepository.GetAll().AsNoTracking()
            .Where(x => x.IsPaid == true && x.CreatedAt.Value.Year == request.Year)
            .GroupBy(x => x.CreatedAt.Value.Month)
            .Select(group => new
            {
                Month = group.Key,
                Orders = group.Count(), 
                Sales = group.Sum(x => x.TotalPrice)
            })
            .OrderBy(group => group.Month)
            .ToListAsync(cancellationToken);

        var totalSales = sales.Sum(x => x.Sales);
        
        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalSales = totalSales,
            SalesGroupByMoth = sales
        });
    }
}