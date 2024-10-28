using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.UoW;

namespace Application.Breeds.Commands.UpdateBreed;

public record UpdateBreedCommand : IRequest<ResponseModel>
{
    public Guid Id { get; init; }
    public BreedUpdateRequestModel UpdateBreedModel { get; init; }
}

public class UpdateBreedCommandHandler : IRequestHandler<UpdateBreedCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBreedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(UpdateBreedCommand request, CancellationToken cancellationToken)
    {
        var breed = await _unitOfWork.BreedRepository.GetByIdAsync(request.Id);
        if (breed is null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Breed not found");
        }

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            breed.Name = request.UpdateBreedModel.Name ?? breed.Name;
            breed.Description = request.UpdateBreedModel.Description ?? breed.Description;
            _unitOfWork.BreedRepository.Update(breed);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Update breed successfully.", breed.Id);
            }

            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update breed failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Update breed failed.", e.Message);
        }

    }
}