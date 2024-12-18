﻿using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.ThirdPartyManager.Cloudinary;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Products.Commands.UpdateTankProduct;

public record UpdateTankProductCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public TankProductUpdateModel TankProductUpdateModel { get; init; }
}

public class UpdateTankProductCommandHandler : IRequestHandler<UpdateTankProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTankProductCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateTankProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetProductIncludeTankById(request.ProductId);
        if (product?.Tank is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found");
        }


        // product
        product.Name = request.TankProductUpdateModel.Name ?? product.Name;
        product.Description = request.TankProductUpdateModel.Description ?? product.Description;
        product.DescriptionDetail = request.TankProductUpdateModel.DescriptionDetail ?? product.DescriptionDetail;
        product.StockQuantity = request.TankProductUpdateModel.StockQuantity ?? product.StockQuantity;
        product.Price = request.TankProductUpdateModel.Price ?? product.Price;
        product.OriginalPrice = request.TankProductUpdateModel.OriginalPrice ?? product.OriginalPrice;

        if (product.OriginalPrice > product.Price)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Price must be greater than OriginalPrice");
        }
        
        // tank
        if (request.TankProductUpdateModel.TankModel is not null)
        {
            product.Tank.Size = request.TankProductUpdateModel.TankModel.Size ?? product.Tank.Size;
            product.Tank.SizeInformation = request.TankProductUpdateModel.TankModel.SizeInformation ??
                                           product.Tank.SizeInformation;
            product.Tank.GlassType = request.TankProductUpdateModel.TankModel.GlassType ?? product.Tank.GlassType;

            if (request.TankProductUpdateModel.TankModel.DeleteCategories.Any())
            {
                var deleteCategories =
                    await _unitOfWork.TankCategoryRepository.GetTankCategoriesByIdAsync(request.TankProductUpdateModel.TankModel
                        .DeleteCategories);
                foreach (var category in deleteCategories)
                {
                    product.Tank.TankCategories.Remove(category);
                }
            }

            if (request.TankProductUpdateModel.TankModel.UpdateCategories.Any())
            {
                var updateTankCategories =
                    await _unitOfWork.TankCategoryRepository.GetTankCategoriesByIdAsync(request.TankProductUpdateModel.TankModel
                        .UpdateCategories);
                foreach (var category in
                         updateTankCategories.Where(category => !product.Tank.TankCategories.Contains(category)))
                {
                    product.Tank.TankCategories.Add(category);
                }
            }

        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.ProductRepository.Update(product);
            if (request.TankProductUpdateModel.TankModel is not null)
            {
                _unitOfWork.TankRepository.Update(product.Tank);

            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Update tank product successfully.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}