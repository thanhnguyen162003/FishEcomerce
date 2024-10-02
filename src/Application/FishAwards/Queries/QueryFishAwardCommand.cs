using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.UoW;
namespace Application.FishAwards.Queries;
#pragma warning disable
public record QueryFishAwardCommand : IRequest<PaginatedList<FishAwardResponseModel>>
{
    public FishAwardQueryFilter QueryFilter;
}

public class QueryFishAwardCommandHandler : IRequestHandler<QueryFishAwardCommand, PaginatedList<FishAwardResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryFishAwardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedList<FishAwardResponseModel>> Handle(QueryFishAwardCommand request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber == 0 ? 1 : request.QueryFilter.PageNumber; //default page = 1
        request.QueryFilter.PageSize = request.QueryFilter.PageSize == 0 ? 12 : request.QueryFilter.PageSize; //default page size = 12
        request.QueryFilter.Search = request.QueryFilter.Search == null ? "" : request.QueryFilter.Search;
        var breedList = _unitOfWork.FishAwardRepository.GetAll();

        if (!breedList.Any())
        {
            return new PaginatedList<FishAwardResponseModel>(new List<FishAwardResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<FishAwardResponseModel>>(breedList);
        return PaginatedList<FishAwardResponseModel>.Create(mapperList, request.QueryFilter.PageNumber, request.QueryFilter.PageSize);
    }
}