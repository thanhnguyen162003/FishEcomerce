using System.Net;
using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;

namespace Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand : IRequest<ResponseModel>
{
    public Guid CategoryId { get; set; }
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
        if (request.CategoryUpdateModel.Name is not null)
        {
            var check = await _unitOfWork.CategoryRepository.ExistsByName(request.CategoryUpdateModel.Name.Trim());
            if (check)
            {
                return new ResponseModel(HttpStatusCode.BadRequest, "Category with the given Name already exists.");
            }
        }

        var category = await _unitOfWork.CategoryRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.DeletedAt == null, cancellationToken);

        if (category is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Category not found.");
        }

        category.Name = request.CategoryUpdateModel.Name ?? category.Name;
        category.Type = request.CategoryUpdateModel.Type == null
            ? category.Type
            : request.CategoryUpdateModel.Type.ToString();
        
        category.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Category updated.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}