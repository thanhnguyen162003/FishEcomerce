using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.UoW;
namespace Application.Breeds.QueryBreed;
#pragma warning disable
public record QueryBreedCommand : IRequest<PaginatedList<BreedResponseModel>>
{
    public BreedQueryFilter QueryFilter;
}

public class QueryBreedCommandHandler : IRequestHandler<QueryBreedCommand, PaginatedList<BreedResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public QueryBreedCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginatedList<BreedResponseModel>> Handle(QueryBreedCommand request, CancellationToken cancellationToken)
    {
        request.QueryFilter.PageNumber = request.QueryFilter.PageNumber == 0 ? 1 : request.QueryFilter.PageNumber; //default page = 1
        request.QueryFilter.PageSize = request.QueryFilter.PageSize == 0 ? 12 : request.QueryFilter.PageSize; //default page size = 12
        request.QueryFilter.Search = request.QueryFilter.Search == null ? "" : request.QueryFilter.Search;
        var breedList = await _unitOfWork.BreedRepository.GetBreedByName(request.QueryFilter.Search, request.QueryFilter.PageSize, request.QueryFilter.PageNumber);

        if (!breedList.Any())
        {
            return new PaginatedList<BreedResponseModel>(new List<BreedResponseModel>(), 0, 0, 0);
        }
        var mapperList = _mapper.Map<List<BreedResponseModel>>(breedList);
        return await PaginatedList<BreedResponseModel>.CreateAsync(mapperList, request.QueryFilter.PageNumber, request.QueryFilter.PageSize);
    }
}