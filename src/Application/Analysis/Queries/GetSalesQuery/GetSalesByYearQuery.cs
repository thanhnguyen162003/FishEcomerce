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
        var sales = await _unitOfWork.OrderRepository.GetAll()
            .Where(x => x.IsPaid == true && x.CreatedAt.Value.Year == request.Year)
            .SumAsync(x => x.TotalPrice, cancellationToken);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalSales = sales,
        });
    }
}