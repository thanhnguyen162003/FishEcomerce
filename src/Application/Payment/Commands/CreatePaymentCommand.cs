using System.Net;
using Application.Common.Models;
using Application.Common.Models.PaymentModels;
using Application.Common.ThirdPartyManager.PayOS;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.Payment.Commands;

public record CreatePaymentCommand : IRequest<ResponseModel>
{
   public long OrderCode { get; init; }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, ResponseModel>
{
   private readonly IPayOSService _payOsService;
   private readonly IUnitOfWork _unitOfWork;
   private readonly IClaimsService _claimsService;

   public CreatePaymentCommandHandler(IPayOSService payOsService, IUnitOfWork unitOfWork, IClaimsService claimsService)
   {
      _payOsService = payOsService;
      _unitOfWork = unitOfWork;
      _claimsService = claimsService;
   }

   public async Task<ResponseModel> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
   {
      var order = await _unitOfWork.OrderRepository.GetOrderByOrderCode(request.OrderCode);

      if (order is null)
      {
         return new ResponseModel(HttpStatusCode.NotFound,"Order not found.");
      }

      // await _payOsService.CancelPayment(request.OrderCode);
      order.OrderCode = int.Parse(DateTimeOffset.Now.ToString("fffff"));
      
      var description = $"AQUA{order.OrderCode}";

      var payment = new PaymentRequestModel
      {
         OrderCode = order.OrderCode!.Value,
         Address = order.ShipAddress,
         Description = description,
         TotalPrice = (decimal) order.TotalPrice!,
         FullName = _claimsService.GetCurrentFullname
      };
      
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
         throw;
      }
      
      var paymentLink = await _payOsService.CreatePayment(payment);
         
      return new ResponseModel(HttpStatusCode.OK, "Payment link created successfully", new { paymentLink = paymentLink });
   }
}