using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Tanks.Commands.CreateTank;

public record CreateTankCommand : IRequest<ResponseModel>
{
    public Guid ProductId { get; init; }
    public TankCreateModel TankModel { get; init; }
}

public class CreateTankCommandHandler : IRequestHandler<CreateTankCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public async Task<ResponseModel> Handle(CreateTankCommand request, CancellationToken cancellationToken)
    {
        var id = new UuidV7().Value;
        
        var tank = _mapper.Map<Tank>(request.TankModel);
        tank.Id = id;
        tank.ProductId = request.ProductId;
        // not add category

        await _unitOfWork.BeginTransactionAsync(); 
        try
        {
            await _unitOfWork.TankRepository.AddAsync(tank, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Create tank successfully.");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create tank failed.");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create tank failed.");
        }
        
    }
}