using System.Net;
using Application.Common.Models;
using Application.Common.ThirdPartyManager.PayOS;
using Application.Common.UoW;
using Domain.Constants;
using Net.payOS.Types;

namespace Application.Payment.Commands;

public record WebhookCommand : IRequest
{
    public WebhookType Type { get; init; }
}

public class WebhookCommandHanlder : IRequestHandler<WebhookCommand>
{
    private readonly IPayOSService _payOsService;
    private readonly IUnitOfWork _unitOfWork;

    public WebhookCommandHanlder(IPayOSService payOsService, IUnitOfWork unitOfWork)
    {
        _payOsService = payOsService;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(WebhookCommand request, CancellationToken cancellationToken)
    {
        var check = _payOsService.SignatureValidate(request.Type);
        if (!check)
        {
            return;
        }
        
        var order = await _unitOfWork.OrderRepository.GetOrderByOrderCode(request.Type.data.orderCode);

        if (order is null)
        {
            return;
        }

        order.IsPaid = true;
        order.UpdatedAt = DateTime.Now;
        
        //update stock
        var productIds = order.OrderDetails.Select(z => z.ProductId);
        var productList = await _unitOfWork.ProductRepository.GetProductsByOrderDetailIds(productIds);
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
            throw;
        }
    }
}