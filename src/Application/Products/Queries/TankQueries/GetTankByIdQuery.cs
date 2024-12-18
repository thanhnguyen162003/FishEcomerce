using System.Net;
using Application.Common.Models;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Products.Queries.FishQueries;

public record GetTankByIdQuery : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
}

public class GetTankByIdQueryHandler : IRequestHandler<GetTankByIdQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClaimsService _claimsService;

    public GetTankByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(GetTankByIdQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.ProductRepository.GetAll()
            .Where(x => x.DeletedAt == null)
            .Include(x => x.Tank)
            .Include(x => x.Tank.TankCategories)
            .Include(x => x.Images)
            .Include(x => x.Staff)
            .Include(x => x.Feedbacks)
            .AsNoTracking().AsSplitQuery();

        if (string.IsNullOrEmpty(_claimsService.GetCurrentRole) || _claimsService.GetCurrentRole.Equals("Customer"))
        {
            queryable = queryable.Where(x => x.StockQuantity > 0);
        }
        
        var product = await queryable.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (product is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Product not found.");
        }
        
        var mapper = _mapper.Map<ProductResponseModel>(product);
        return new ResponseModel(HttpStatusCode.OK,"", mapper);
    }
}