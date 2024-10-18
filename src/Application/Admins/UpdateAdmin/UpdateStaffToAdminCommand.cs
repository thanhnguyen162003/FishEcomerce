using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Admins.UpdateAdmin;

public record UpdateStaffToAdminCommand : IRequest<ResponseModel>
{
    public Guid StaffId { get; init; }
}

public class UpdateStaffToAdminCommandHandler : IRequestHandler<UpdateStaffToAdminCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStaffToAdminCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateStaffToAdminCommand request, CancellationToken cancellationToken)
    {
        var staff = await _unitOfWork.StaffRepository.GetStaffByIdAsync(request.StaffId);

        if (staff is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Staff could not be found or has been deleted.");
        }
        
        staff.IsAdmin = true;
        staff.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.StaffRepository.Update(staff);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Staff updated to admin.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}