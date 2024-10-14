using System.Net;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Models;
using Application.Common.Models.PaymentModels;
using Application.Common.ThirdPartyManager.PayOS;
using Application.Common.UoW;
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

    public WebhookCommandHanlder(IPayOSService payOsService, IUnitOfWork unitOfWork)
    {
        _payOsService = payOsService;
        _unitOfWork = unitOfWork;
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

        await _unitOfWork.BeginTransactionAsync();
        try
        { 
            _unitOfWork.OrderRepository.Update(order);
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