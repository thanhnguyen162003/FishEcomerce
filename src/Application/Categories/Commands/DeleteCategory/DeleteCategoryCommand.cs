using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand : IRequest<ResponseModel>
{
    public Guid CategoryId { get; init; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseModel> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.CategoryRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId && x.DeletedAt == null, cancellationToken);

        if (category is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Category not found");
        }
        
        category.DeletedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.CategoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Category deleted");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}