using System.Net;
using Application.Common.Models;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Auth.Commands.UpdatePassword;

public record UpdatePasswordCommand : IRequest<ResponseModel>
{
    public PasswordUpdateModel PasswordUpdateModel { get; init; }
}

public class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public UpdatePasswordCommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _claimsService.GetCurrentUserId;
        var role = _claimsService.GetCurrentRole;
        if (role.Equals("Customer"))
        {
            var customer = await _unitOfWork.CustomerRepository.GetCustomerById(userId);
            if (customer is null)
            {
                return new ResponseModel(HttpStatusCode.NotFound, "Your account not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.PasswordUpdateModel.OldPassword, customer.Password))
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Old password is incorrect.");
            }
            
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.PasswordUpdateModel.NewPassword);
            customer.Password = hashedPassword;
            customer.UpdatedAt = DateTime.Now;
        }
        else
        {
            var supplier = await _unitOfWork.StaffRepository.GetStaffByIdAsync(userId);
            if (supplier is null)
            {
                return new ResponseModel(HttpStatusCode.NotFound, "Your account not found.");
            }
        
            if (!BCrypt.Net.BCrypt.Verify(request.PasswordUpdateModel.OldPassword, supplier.Password))
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Old password is incorrect.");
            }
        
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.PasswordUpdateModel.NewPassword);
            supplier.Password = hashedPassword;
            supplier.UpdatedAt = DateTime.Now;
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Password updated successfully.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}