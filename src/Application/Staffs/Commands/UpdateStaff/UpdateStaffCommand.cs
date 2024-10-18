using System.Net;
using Application.Common.Models;
using Application.Common.Models.StaffModels;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.Staffs.Commands.UpdateStaff;

public record UpdateStaffCommand : IRequest<ResponseModel>
{
    public StaffUpdateModel StaffUpdateModel { get; init; }
}

public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public UpdateStaffCommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var staffId = _claimsService.GetCurrentUserId;
        var staff = await _unitOfWork.StaffRepository.GetStaffByIdAsync(staffId);

        if (staff is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Staff could not be found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.StaffUpdateModel.Password, staff.Password))
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Wrong password.");
        }
        
        staff.Facebook = request.StaffUpdateModel.Facebook ?? staff.Facebook;
        staff.FullName = request.StaffUpdateModel.FullName ?? staff.FullName;
        staff.UpdatedAt = DateTime.Now;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.StaffRepository.Update(staff);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Staff updated.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}