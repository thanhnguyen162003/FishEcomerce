using System.Net;
using Application.Common.Models;
using Application.Common.Models.StaffModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Constants;
using Domain.Entites;
using Microsoft.Extensions.Options;

namespace Application.Admins.Commands.CreateStaff;

public record CreateStaffCommand : IRequest<ResponseModel>
{
    public StaffCreateModel StaffCreateModel { get; init; }
}

public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DefaultPassword _defaultPassword;
    private readonly IClaimsService _claimsService;

    public CreateStaffCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IOptions<DefaultPassword> defaultPassword, IClaimsService claimsService)
    {
        _claimsService = claimsService;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _defaultPassword = defaultPassword.Value;
    }

    public async Task<ResponseModel> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var staffId = _claimsService.GetCurrentUserId;
        var isAdmin = await _unitOfWork.StaffRepository.CheckAdminAsync(staffId);

        if (!isAdmin)
        {
            return new ResponseModel(HttpStatusCode.Forbidden, "You are not allowed to create new staff.");
        }
        
        var staffExisted = await _unitOfWork.StaffRepository.CheckUsernameAsync(request.StaffCreateModel.Username!);

        if (staffExisted)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Username is already taken.");
        }
        
        var staff = _mapper.Map<Staff>(request.StaffCreateModel);
        
        staff.Id = new UuidV7().Value;
        staff.Password = BCrypt.Net.BCrypt.HashPassword(_defaultPassword.Password);
        staff.CreatedAt = DateTime.Now;
        staff.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.StaffRepository.AddAsync(staff, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Staff created successfully.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}