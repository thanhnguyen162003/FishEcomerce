using System.Net;
using Application.Common.Models;
using Application.Common.Models.TankCategoryModels;
using Application.Common.UoW;

namespace Application.TankCategories.Commands.UpdateTankCategory;

public record UpdateTankCategoryCommand : IRequest<ResponseModel>
{
    public Guid TankCategoryId { get; init; }
    public TankCategoryUpdateModel TankCategoryUpdateModel { get; init; }
}

public class UpdateTankCategoryCommandHandler : IRequestHandler<UpdateTankCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTankCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateTankCategoryCommand request, CancellationToken cancellationToken)
    {
        var tankCategory = await _unitOfWork.TankCategoryRepository.GetByIdAsync(request.TankCategoryId);

        if (tankCategory is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Category not found.");
        }
        
        tankCategory.Level = request.TankCategoryUpdateModel.Level ?? tankCategory.Level;
        tankCategory.TankType = request.TankCategoryUpdateModel.TankType ?? tankCategory.TankType;
        tankCategory.UpdatedAt = DateTime.Now;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.TankCategoryRepository.Update(tankCategory);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Category updated.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Category NOT updated.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
    }
}