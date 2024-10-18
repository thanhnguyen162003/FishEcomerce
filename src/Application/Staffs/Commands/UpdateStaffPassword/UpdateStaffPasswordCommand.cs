using System.Net;
using Application.Common.Models;
using Application.Common.Models.StaffModels;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.Staffs.Commands.UpdateStaffPassword;

public record UpdateStaffPasswordCommand : IRequest<ResponseModel>
{
    public StaffPasswordUpdateModel StaffPasswordUpdateModel { get; init; }
}

public class UpdateStaffPasswordCommandHandler : IRequestHandler<UpdateStaffPasswordCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public UpdateStaffPasswordCommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(UpdateStaffPasswordCommand request, CancellationToken cancellationToken)
    {
        var staffId = _claimsService.GetCurrentUserId;
        var staff = await _unitOfWork.StaffRepository.GetStaffByIdAsync(staffId);

        if (staff is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Staff could not be found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.StaffPasswordUpdateModel.OldPassword, staff.Password))
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Wrong password.");
        }
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.StaffPasswordUpdateModel.NewPassword);
        staff.Password = hashedPassword;
        staff.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        { 
            _unitOfWork.StaffRepository.Update(staff);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Password updated.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}