using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Application.Common.UoW;

namespace Application.FishAwards.Commands.UpdateFishAward;

public record UpdateFishAwardCommand : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
    public FishAwardUpdateRequestModel UpdateFishAwardModel { get; init; }
}

public class UpdateFishAwardCommandHandler : IRequestHandler<UpdateFishAwardCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFishAwardCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateFishAwardCommand request, CancellationToken cancellationToken)
    {
        var fishAward = await _unitOfWork.FishAwardRepository.GetByIdAsync(request.Id);
        if (fishAward is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Breed not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            fishAward.Name = request.UpdateFishAwardModel.Name ?? fishAward.Name;
            fishAward.Description = request.UpdateFishAwardModel.Description ?? fishAward.Description;
            fishAward.AwardDate = DateOnly.FromDateTime(request.UpdateFishAwardModel.AwardDate);
            _unitOfWork.FishAwardRepository.Update(fishAward);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Update fish award successfully.", fishAward.Id);
            }

            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update fish award failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update fish award failed.", e.Message);
        }

    }
}