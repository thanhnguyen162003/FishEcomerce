using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Domain.Entites;

namespace Application.Products.Queries.TankQueries;

public record GetTankByIdQuery : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
}

public class GetTankByIdQueryHandler : IRequestHandler<GetTankByIdQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTankByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(GetTankByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _unitOfWork.ProductRepository.GetAll()
            .Include(x => x.Tank)
            .Include(x => x.Tank.Categories)
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