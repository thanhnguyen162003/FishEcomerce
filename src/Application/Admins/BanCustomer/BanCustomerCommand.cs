using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Admins.BanCustomer;

public record BanCustomerCommand : IRequest<ResponseModel>
{
    public Guid CustomerId { get; init; }
}

public class BanCustomerCommandHandler : IRequestHandler<BanCustomerCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public BanCustomerCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(BanCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _unitOfWork.CustomerRepository.GetCustomerById(request.CustomerId);

        if (customer is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Customer not found or has been suspended.");
        }
        
        customer.DeletedAt = DateTime.Now;
        customer.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Customer has been banned.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }    
    }
}