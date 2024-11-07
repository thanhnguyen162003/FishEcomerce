using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.TankCategories.Commands.DeleteTankCategory;

public record DeleteTankCategoryCommand : IRequest<ResponseModel>
{
    public Guid TankCategoryId { get; init; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteTankCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(DeleteTankCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.TankCategoryRepository.GetByIdAsync(request.TankCategoryId);

        if (category is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Category not found.");
        }
        
        category.DeletedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.TankCategoryRepository.Update(category);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Category deleted successfully.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Category deleted failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadGateway, e.Message);
        }
    }
}