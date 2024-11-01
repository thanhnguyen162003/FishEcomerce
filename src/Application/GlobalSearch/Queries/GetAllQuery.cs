using System.Net;
using Application.Common.Models;
using Application.Common.Models.BlogModel;
using Application.Common.Models.ProductModels;
using Application.Common.UoW;

namespace Application.GlobalSearch.Queries;

public record GetAllQuery : IRequest<ResponseModel>
{
    public string Query { get; init; }
}

public class GetAllQueryHandler : IRequestHandler<GetAllQuery, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return new ResponseModel(HttpStatusCode.OK, "", new
            {
                fish = new SearchResponseModel<ProductResponseModel>([]),
                tank = new SearchResponseModel<ProductResponseModel>([]),
                blog = new SearchResponseModel<BlogResponseModel>([])
            });
        }
        
        var fish = await _unitOfWork.ProductRepository.SearchProducts(request.Query, "Fish", cancellationToken);
        var tank = await _unitOfWork.ProductRepository.SearchProducts(request.Query, "Tank",cancellationToken);
        var blog = await _unitOfWork.BlogRepository.SearchBlogs(request.Query, cancellationToken);
        
        var fishDto = _mapper.Map<IEnumerable<ProductResponseModel>>(fish);
        var tankDto = _mapper.Map<IEnumerable<ProductResponseModel>>(tank);
        var blogDto = _mapper.Map<IEnumerable<BlogResponseModel>>(blog);

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
           fish = new SearchResponseModel<ProductResponseModel>(fishDto),
           tank = new SearchResponseModel<ProductResponseModel>(tankDto),
           blog = new SearchResponseModel<BlogResponseModel>(blogDto)
        });
    }
}