using System.Net;
using Application.Common.Models;
using Application.Common.Models.TankCategoryModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.TankCategories.Commands.CreateTankCategory;

public record CreateTankCategoryCommand : IRequest<ResponseModel>
{
    public TankCategoryCreateModel TankCategoryCreateModel { get; init; }
}

public class CreateTankCategoryCommandHandler : IRequestHandler<CreateTankCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTankCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateTankCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = new UuidV7().Value;
        
        var category = _mapper.Map<TankCategory>(request.TankCategoryCreateModel);
        category.Id = categoryId;
        category.CreatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.TankCategoryRepository.AddAsync(category, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Category created successfully.");
            }
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Category creation failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }

    }
}