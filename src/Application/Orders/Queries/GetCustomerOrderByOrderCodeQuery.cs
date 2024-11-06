using System.Net;
using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.Orders.Queries;

public record GetCustomerOrderByOrderCodeQuery : IRequest<ResponseModel>
{
    public long OrderCode { get; init; }
}

public class GetCustomerOrderByOrderCodeQueryHandler : IRequestHandler<GetCustomerOrderByOrderCodeQuery, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public GetCustomerOrderByOrderCodeQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(GetCustomerOrderByOrderCodeQuery request, CancellationToken cancellationToken)
    {
        var customerId = _claimsService.GetCurrentUserId;
        var order = await _unitOfWork.OrderRepository.GetAll().AsNoTracking()
            .Include(x => x.OrderDetails)
            .ThenInclude(y => y.Product)
            .Where(x => x.OrderCode == request.OrderCode && x.CustomerId == customerId)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken);

        if (order is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Order not found");
        }
        
        var mapper = _mapper.Map<OrderResponseModel>(order);
        
        return new ResponseModel(HttpStatusCode.OK, "", mapper);
    }
}