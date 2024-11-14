using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;

namespace Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public ProductUpdateModel ProductUpdateModel { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductIncludeTankById(request.ProductId);
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }
        
        product.Name = request.ProductUpdateModel.Name ?? product.Name;
        product.Description = request.ProductUpdateModel.Description ?? product.Description;
        product.DescriptionDetail = request.ProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
        product.StockQuantity = request.ProductUpdateModel.StockQuantity ?? product.StockQuantity;
        product.Price = request.ProductUpdateModel.Price ?? product.Price;
        product.OriginalPrice = request.ProductUpdateModel.OriginalPrice ?? product.OriginalPrice;
        product.Type = request.ProductUpdateModel.Type == null ? product.Type : request.ProductUpdateModel.Type.ToString()?.ToLower();

        if (request.ProductUpdateModel.DeleteCategories != null && request.ProductUpdateModel.DeleteCategories.Any())
        {
            var deleteCategories = await _unitOfWork.CategoryRepository.GetCategoriesByIdAsync(request.ProductUpdateModel.DeleteCategories);
            foreach (var category in deleteCategories)
            {
                product.Categories.Remove(category);
            }
        }
        
        if (request.ProductUpdateModel.UpdateCategories != null && request.ProductUpdateModel.UpdateCategories.Any())
        {
            var categories = await _unitOfWork.CategoryRepository.GetCategoriesByIdAsync(request.ProductUpdateModel.UpdateCategories);
            foreach (var category in categories.Where(category => !product.Categories.Contains(category)))
            {
                product.Categories.Add(category);
            }
        }
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.ProductRepository.Update(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Update product successfully.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}