using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.CustomerFeature.Commands.UpdateCustomer;

public record UpdateCustomerCommand : IRequest<ResponseModel>
{
    public CustomerUpdateModel CustomerUpdateModel { get; init; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public UpdateCustomerCommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerId = _claimsService.GetCurrentUserId;
        var customer = await _unitOfWork.CustomerRepository.GetCustomerById(customerId);

        if (customer is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Customer not found.");
        }

        customer.UpdatedAt = DateTime.Now;
        customer.Name = request.CustomerUpdateModel.Name ?? customer.Name;
        customer.Phone = request.CustomerUpdateModel.Phone ?? customer.Phone;
        customer.Address = request.CustomerUpdateModel.Address ?? customer.Address;
        customer.Birthday = request.CustomerUpdateModel.Birthday ?? customer.Birthday;

        await _unitOfWork.BeginTransactionAsync();

        try
        { 
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Update customer successful.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}