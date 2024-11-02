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
            .Where(x => x.IsPaid == true && x.CreatedAt.Value.Month == request.Month &&
                        x.CreatedAt.Value.Year == request.Year)
            .SumAsync(x => x.TotalPrice, cancellationToken);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalSales = sales,
        });
    }
}