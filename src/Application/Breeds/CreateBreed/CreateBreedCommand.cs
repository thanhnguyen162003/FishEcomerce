using System.Net;
using Application.Common.Models;
using Application.Common.Models.BreedModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Breeds.CreateBreed;

public record CreateBreedCommand : IRequest<ResponseModel>
{
    public BreedCreateRequestModel CreateBreedModel { get; init; }
}

public class CreateBreedCommandHandler : IRequestHandler<CreateBreedCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateBreedCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateBreedCommand request, CancellationToken cancellationToken)
    {
        // product
        var breedId = new UuidV7().Value;
        var breed = _mapper.Map<Breed>(request.CreateBreedModel);
        breed.Id = breedId;
        breed.CreatedAt = DateTime.Now;
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.BreedRepository.AddAsync(breed, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.Created, "Create breed successfully.", breed.Id);
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create breed failed.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create breed failed.", e.Message);
        }
    }
}