using System.Net;
using Application.Common.Models;
using Application.Common.Models.FishAwardModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.FishAwards.Commands.CreateFishAward;

public record CreateFishAwardCommand : IRequest<ResponseModel>
{
    public FishAwardCreateRequestModel CreateFishAwardModel { get; init; }
    public Guid Id { get; set; }
}

public class CreateFishAwardCommandHandler : IRequestHandler<CreateFishAwardCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateFishAwardCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateFishAwardCommand request, CancellationToken cancellationToken)
    {
        var fish = _unitOfWork.FishRepository.GetByIdAsync(request.Id);
        if (fish == null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Fish not exist", request.Id);
        }
        // award
        var fishAward = new FishAward()
        {
            Id = new UuidV7().Value,
            Name = request.CreateFishAwardModel.Name,
            FishId = request.Id,
            Description = request.CreateFishAwardModel.Description,
            AwardDate = DateOnly.FromDateTime(request.CreateFishAwardModel.AwardDate),
        };

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.FishAwardRepository.AddAsync(fishAward, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create fish award successfully.", fishAward.Id);
            }

            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create fish award failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create fish award failed.", e.Message);
        }
    }
}