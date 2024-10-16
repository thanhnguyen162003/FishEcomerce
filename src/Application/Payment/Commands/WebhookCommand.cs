using System.Net;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Models;
using Application.Common.Models.PaymentModels;
using Application.Common.ThirdPartyManager.PayOS;
using Application.Common.UoW;
using Domain.Entites;
using Domain.Enums;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;

namespace Application.Payment.Commands;

public record WebhookCommand : IRequest<ResponseModel>
{
    public WebhookType Type { get; init; }
}

public class WebhookCommandHanlder : IRequestHandler<WebhookCommand, ResponseModel>
{
    private readonly IPayOSService _payOsService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WebhookCommandHanlder(IPayOSService payOsService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _payOsService = payOsService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(WebhookCommand request, CancellationToken cancellationToken)
    {
        var check = _payOsService.SignatureValidate(request.Type);
        if (!check)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Payment fail!");
        }
        
        var order = await _unitOfWork.OrderRepository.GetOrderByOrderCode(request.Type.data.orderCode);

        if (order is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Order not found");
        }
        
        order.Status = OrderStatus.Pending.ToString();
        order.IsPaid = true;
        
        //update stock
        var orderDetailIds = order.OrderDetails.Select(z => z.Id);
        var productList = await _unitOfWork.ProductRepository.GetProductsByOrderDetailIds(orderDetailIds);
        var orderDetailDictionary = order.OrderDetails.ToDictionary(od => od.ProductId);

        foreach (var product in productList)
        {
            if (orderDetailDictionary.TryGetValue(product.Id, out var orderDetail))
            {
                product.StockQuantity -= orderDetail.Quantity;
            }
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        { 
            _unitOfWork.OrderRepository.Update(order);
            _unitOfWork.ProductRepository.UpdateRange(productList.ToList());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
        return new ResponseModel(HttpStatusCode.BadRequest, "Payment fail!");
    }
}