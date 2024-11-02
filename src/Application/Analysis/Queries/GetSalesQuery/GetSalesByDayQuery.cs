using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetSalesQuery;

public record GetSalesByDayQuery : IRequest<ResponseModel>
{
    public int Day { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
}

public class GetSalesByDayQueryHandler : IRequestHandler<GetSalesByDayQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSalesByDayQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(GetSalesByDayQuery request, CancellationToken cancellationToken)
    {
        if (!DateTime.TryParse($"{request.Year}-{request.Month:D2}-{request.Day:D2}", out var date))
        {
            return new ResponseModel(HttpStatusCode.BadRequest, $"Invalid date: {request.Day}");
        }

        var sales = await _unitOfWork.OrderRepository.GetAll()
            .Where(x => x.IsPaid == true && x.CreatedAt == date)
            .SumAsync(x => x.TotalPrice, cancellationToken);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalSales = sales,
        });
    }
}