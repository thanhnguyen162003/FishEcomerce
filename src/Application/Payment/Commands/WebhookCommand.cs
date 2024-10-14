using System.Net;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Models;
using Application.Common.Models.PaymentModels;
using Application.Common.ThirdPartyManager.PayOS;
using Application.Common.UoW;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json.Linq;

namespace Application.Payment.Commands;

public record WebhookCommand : IRequest<ResponseModel>
{
    public WebhookModel WebhookModel { get; init; }
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
        if (!request.WebhookModel.Success)
        {
            return new ResponseModel(HttpStatusCode.BadRequest,"Payment fail!");
        }
        var check =
            await _payOsService.SignatureValidate(request.WebhookModel.OrderCode, request.WebhookModel.Signature);
        if (check)
        {
            
        }
        return new ResponseModel(HttpStatusCode.BadRequest,"Payment fail!");

    }
    
    
}