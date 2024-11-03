using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.UoW;
using Microsoft.Extensions.Options;

namespace Application.Breeds.Queries;
#pragma warning disable
public record GetBreedsQuery : IRequest<PaginatedList<BreedResponseModel>>
{
    public BreedQueryFilter QueryFilter;
}

public class GetBreedsQueryCommandHandler : IRequestHandler<GetBreedsQuery, PaginatedList<BreedResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;

    public GetBreedsQueryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
    }

    public async Task<PaginatedList<BreedResponseModel>> Handle(GetBreedsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _mapper.ProjectTo<BreedResponseModel>(_unitOfWork.BreedRepository.GetAll().Where(x => x.DeletedAt == null).AsNoTracking());
        
        if (request.QueryFilter.PageNumber is not null && request.QueryFilter.PageSize is not null)
        {
            request.QueryFilter.PageSize = request.QueryFilter.PageSize <= 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }

        queryable = queryable.OrderBy(x => x.Name).ThenBy(x => x.Id);
        var breeds = await queryable.ToListAsync(cancellationToken);
        var count  = await queryable.CountAsync(cancellationToken);

        return count == 0
            ? new PaginatedList<BreedResponseModel>([], 0, 0, 0)
            : new PaginatedList<BreedResponseModel>(breeds, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }
}