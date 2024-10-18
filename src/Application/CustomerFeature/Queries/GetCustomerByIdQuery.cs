using System.Net;
using Application.Common.Models;
using Application.Common.Models.CustomerModels;
using Application.Common.UoW;
using Application.Common.Utils;

namespace Application.CustomerFeature.Queries;

public record GetCustomerByIdQuery : IRequest<ResponseModel>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ResponseModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClaimsService _claimsService;

    public GetCustomerByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IClaimsService claimsService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _claimsService = claimsService;
    }

    public async Task<ResponseModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customerId = _claimsService.GetCurrentUserId;
        var customer = await _unitOfWork.CustomerRepository.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);

        if (customer is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Customer not found.");
        }
        
        var mapper = _mapper.Map<CustomerResponseModel>(customer);
        return new ResponseModel(HttpStatusCode.OK, "", mapper);
    }
}