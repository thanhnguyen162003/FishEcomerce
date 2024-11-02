using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;

namespace Application.AnalysisFeature.Queries.GetRegisterQuery;

public record GetRegisterByDayQuery : IRequest<ResponseModel>
{
    public int Day { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
}

public class GetRegisterByDayQueryHandler : IRequestHandler<GetRegisterByDayQuery, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetRegisterByDayQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(GetRegisterByDayQuery request, CancellationToken cancellationToken)
    {
        if (!DateTime.TryParse($"{request.Year}-{request.Month:D2}-{request.Day:D2}", out var date))
        {
            return new ResponseModel(HttpStatusCode.BadRequest, $"Invalid date: {request.Day}");
        }
        
        var customers = await _unitOfWork.CustomerRepository.GetAll()
            .AsNoTracking()
            .Where(x => x.DeletedAt == null && x.CreatedAt == date)
            .ToListAsync(cancellationToken);
        
        var mapper = _mapper.Map<List<CustomerResponseModel>>(customers);
        var count = mapper.Count;

        return new ResponseModel(HttpStatusCode.OK, "", new
        {
            TotalCustomers = count,
            Customers = mapper
        });
    }
}