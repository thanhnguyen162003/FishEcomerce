using System.Net;
using Application.Common.Models;
using Application.Common.Models.OrderModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;
using Domain.Enums;

namespace Application.Order.Command;

public record CreateOrderCommand : IRequest<ResponseModel>
{
    public OrderCreateModel OrderCreateModel { get; init; }
}

public class OrderCreateModelHandler : IRequestHandler<CreateOrderCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public OrderCreateModelHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Domain.Entites.Order>(request.OrderCreateModel);
        order.Id = new UuidV7().Value;
        order.CreatedAt = DateTime.Now;
        order.Status = OrderStatus.NotPaid.ToString();
        order.CustomerId = _claimsService.GetCurrentUserId;
        decimal totalPrice = 0;
            
        var orderDetails = _mapper.Map<List<OrderDetail>>(request.OrderCreateModel.OrderDetails);

        foreach (var orderDetail in orderDetails)
        {
            totalPrice = (decimal)(totalPrice + orderDetail.TotalPrice);
            orderDetail.Id = new UuidV7().Value;
            orderDetail.OrderId = order.Id;
            var priceCheck = await _unitOfWork.ProductRepository.GetProductPrice((Guid)orderDetail.ProductId);
            if (priceCheck * orderDetail.Quantity != orderDetail.TotalPrice)
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Unit price of product not match");
            }
        }

        if (totalPrice != order.TotalPrice)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Total price not match");
        }

        order.OrderDetails = orderDetails;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.OrderRepository.AddAsync(order, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Order create successfully!");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }

    }
}