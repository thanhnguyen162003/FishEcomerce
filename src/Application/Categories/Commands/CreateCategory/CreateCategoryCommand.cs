using System.Net;
using Application.Common.Models;
using Application.Common.Models.CategoryModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Categories.Commands.CreateCategory;

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
        var check = await _unitOfWork.CategoryRepository.ExistsByName(request.CategoryCreateModel.Name.Trim());

        if (check)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Category already exists");
        }
        
        var category = _mapper.Map<Category>(request.CategoryCreateModel);
        category.Id = new UuidV7().Value;
        category.CreatedAt = DateTime.Now;
        category.UpdatedAt = DateTime.Now;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.CategoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Category created successfully");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}