using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Entites;

namespace Application.Products.Queries.TankQueries;

public record GetFishProductByIdQuery : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
}

public class GetFishProductByIdQueryHandler : IRequestHandler<GetFishProductByIdQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFishProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(GetFishProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAll()
            .Include(x => x.Fish)
            .Include(x => x.Fish.Breed)
            .Include(x => x.Images)
            .Include(x => x.Supplier)
            .AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found.");
        }

        var mapper = _mapper.Map<ProductResponseModel>(product);
        return new ResponseModel(HttpStatusCode.OK,"", mapper);
    }
}