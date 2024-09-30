using System.Net;
using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.TankCategories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<ResponseModel>
{
    public CategoryCreateModel CategoryCreateModel { get; init; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryId = new UuidV7().Value;
        
        var category = _mapper.Map<Category>(request.CategoryCreateModel);
        category.Id = categoryId;
        category.CreatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
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