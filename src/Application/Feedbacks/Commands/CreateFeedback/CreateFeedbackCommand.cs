using System.Net;
using Application.Common.Models;
using Application.Common.Models.FeedbackModels;
using Application.Common.UoW;
using Application.Common.Utils;
using Domain.Entites;

namespace Application.Feedbacks.Commands.CreateFeedback;

public record CreateFeedbackCommand : IRequest<ResponseModel>
{
    public FeedbackCreateModel FeedbackCreateModel { get; init; }
}

public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateFeedbackCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedbackId = new UuidV7().Value;
        var feedback = _mapper.Map<Feedback>(request.FeedbackCreateModel);
        feedback.Id = feedbackId;
        feedback.CreatedAt = DateTime.Now;
        
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await _unitOfWork.FeedbackRepository.AddAsync(feedback, cancellationToken);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                await _unitOfWork.CommitTransactionAsync();
                return new ResponseModel(HttpStatusCode.OK, "Create feedback successfully!");
            }
            
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, "Create feedback failed!");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
    }
}