﻿using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Products.Commands.CreateProduct;

public record CreateTankProductCommand : IRequest<ResponseModel>
{
    public CreateTankProductModel CreateTankProductModel { get; init; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateTankProductCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateTankProductCommand request, CancellationToken cancellationToken)
    {
        // product
        var productId = new UuidV7().Value;
        var slug = SlugHelper.GenerateSlug(request.CreateTankProductModel.Name, productId.ToString());
        var product = _mapper.Map<Product>(request.CreateTankProductModel);
        product.Id = productId;
        product.Slug = slug;
        product.CreatedAt = DateTime.Now;
        // not add images yet

        
        // tank
        var tankId = new UuidV7().Value;
        var tank = _mapper.Map<Tank>(request.CreateTankProductModel.TankModel);
        tank.Id = tankId;
        tank.ProductId = productId;
        // not add category yet

        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.ProductRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.TankRepository.AddAsync(tank, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 1)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create tank product successfully.", product.Id);
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create tank product failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create tank product failed.", e.Message);
        }
    }
}