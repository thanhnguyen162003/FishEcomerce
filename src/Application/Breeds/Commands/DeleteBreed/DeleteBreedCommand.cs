using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Breeds.Commands.DeleteBreed;

public record DeleteBreedCommand : IRequest<ResponseModel>
{
    public Guid Id { get; set; }
}

public class DeleteBreedCommandHandler : IRequestHandler<DeleteBreedCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBreedCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseModel> Handle(DeleteBreedCommand request, CancellationToken cancellationToken)
    {
        var breed = await _unitOfWork.BreedRepository.GetByIdAsync(request.Id);
        if (breed is null)
        {
            return new ResponseModel(HttpStatusCode.BadRequest, "Breed not found.");
        }
        breed.DeletedAt = DateTime.Now;
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.BreedRepository.Update(breed);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Breed has been deleted.");
            }
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Breed has NOT been deleted.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }

    }
}