﻿using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.Models.FishModels;
using Application.Common.Models.ProductModels;
using Application.Common.Models.TankModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Constants;
using Domain.Entites;
using Microsoft.Extensions.Options;

namespace Application.Products.Queries.FishQueries;
#pragma warning disable
public record GetFishsQuery : IRequest<PaginatedList<ProductResponseModel>>
{
    public FishQueryFilter QueryFilter;
}

public class QueryFishProductCommandHandler : IRequestHandler<GetFishsQuery, PaginatedList<ProductResponseModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PaginationOptions _paginationOptions;
    private readonly IClaimsService _claimsService;

    public QueryFishProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IOptions<PaginationOptions> paginationOptions, IClaimsService claimsService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _paginationOptions = paginationOptions.Value;
        _claimsService = claimsService;
    }

    public async Task<PaginatedList<ProductResponseModel>> Handle(GetFishsQuery request, CancellationToken cancellationToken)
    {
        var queryable = _unitOfWork.ProductRepository.GetAllProductIncludeFish();
        
        if (string.IsNullOrEmpty(_claimsService.GetCurrentRole) || _claimsService.GetCurrentRole.Equals("Customer"))
        {
            queryable = queryable.Where(x => x.StockQuantity > 0);
        }
        
        queryable = Filter(queryable, request.QueryFilter);
        queryable = Sort(queryable, request.QueryFilter);
        var count = await queryable.CountAsync(cancellationToken);

        if (request.QueryFilter.PageSize is not null && request.QueryFilter.PageNumber is not null)
        {
            request.QueryFilter.PageNumber = request.QueryFilter.PageNumber < 1 ? _paginationOptions.DefaultPageNumber : request.QueryFilter.PageNumber;
            request.QueryFilter.PageSize = request.QueryFilter.PageSize < 0 ? _paginationOptions.DefaultPageSize : request.QueryFilter.PageSize;
            queryable = queryable.Skip(((int)request.QueryFilter.PageNumber - 1) * (int)request.QueryFilter.PageSize).Take((int)request.QueryFilter.PageSize);
        }
        
        var productList = await queryable.AsNoTracking().AsSplitQuery().ToListAsync(cancellationToken);
        var products = _mapper.Map<List<ProductResponseModel>>(productList);

        return count == 0
            ? new PaginatedList<ProductResponseModel>([], 0, 0, 0)
            : new PaginatedList<ProductResponseModel>(products, count, request.QueryFilter.PageNumber ?? 0,
                request.QueryFilter.PageSize ?? 0);
    }
    private IQueryable<Product> Filter(IQueryable<Product> queryable, FishQueryFilter fishQueryFilter)
    {
        if (fishQueryFilter.PriceFrom.HasValue)
        {
            queryable = queryable.Where(p => p.Price >= fishQueryFilter.PriceFrom);
        }
        if (fishQueryFilter.PriceTo.HasValue)
        {
            queryable = queryable.Where(p => p.Price <= fishQueryFilter.PriceTo);
        }
        if (!string.IsNullOrEmpty(fishQueryFilter.Search))
        {
            var normalInput = Regex.Replace(fishQueryFilter.Search.Trim(), @"\s+", " ").ToLower();
            queryable = queryable.Where(p => p.Name.ToLower().Contains(normalInput));
        }

        if (!string.IsNullOrEmpty(fishQueryFilter.Breed))
        {
            queryable = queryable.Where(p => p.Fish.Breed.Name.ToLower().Equals(fishQueryFilter.Breed.ToLower()));
        }

        return queryable;
    }

    private IQueryable<Product> Sort(IQueryable<Product> queryable, FishQueryFilter fishQueryFilter)
    {
        queryable = fishQueryFilter.Sort.ToLower() switch
        {
            "date" => fishQueryFilter.Direction.ToLower() == "desc" 
                ? queryable.OrderByDescending(p => p.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id)
                : queryable.OrderBy(p => p.CreatedAt).ThenBy(x => x.Price).ThenBy(x => x.Id),
            _ => fishQueryFilter.Direction.ToLower() == "desc"  
                ? queryable.OrderByDescending(p => p.Price).ThenBy(x => x.CreatedAt).ThenBy(x => x.Id)
                : queryable.OrderBy(p => p.Price).ThenBy(x => x.CreatedAt).ThenBy(x => x.Id),
        };
        
        return queryable;

    }
}