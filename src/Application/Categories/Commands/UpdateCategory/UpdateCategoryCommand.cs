using System.Net;
using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;

namespace Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<ResponseModel>
{
    public Guid CategoryId { get; init; }
    public CategoryUpdateModel CategoryUpdateModel { get; init; }
}

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetByIdAsync(request.CategoryId);

        if (category is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Category not found.");
        }
        
        category.Level = request.CategoryUpdateModel.Level ?? category.Level;
        category.TankType = request.CategoryUpdateModel.TankType ?? category.TankType;
        category.UpdatedAt = DateTime.Now;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.CategoryRepository.Update(category);
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