using System.Net;
using FishEcomerce.Application.Common.Models;
using FishEcomerce.Domain.Entities;
using FishEcomerce.Domain.Enums;
using FishEcomerce.Infrastructure.Context;

namespace Microsoft.Extensions.DependencyInjection.Products.Queries;

public record GetProductQuery(Guid Id, int Category) : IRequest<ResponseModel>;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ResponseModel>
{
    private readonly KingFishContext _context;
    private readonly IMapper _mapper;

    public GetProductQueryHandler(KingFishContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(x => x.Supplier)
            .Include(x => x.Feedbacks)
            .Include(x => x.Images)
            .AsQueryable();
        query = request.Category switch
        {
            CategoryConstaint.FISH => query.Include(x => x.FishProducts),
            _ => query.Include(x=> x.FishTank)
        };

        var product = await query.AsSplitQuery().FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return new ResponseModel(HttpStatusCode.OK,"");
    }
}
