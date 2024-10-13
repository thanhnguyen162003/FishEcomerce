using System.Net;
using Application.Common.Models;
using Application.Common.UoW;

namespace Application.Feedbacks.Commands.DeleteFeedback;

public record DeleteFeedbackCommand : IRequest<ResponseModel>
{
    public Guid feedbackId { get; set; }
}

public class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand, ResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFeedbackCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseModel> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback = await _unitOfWork.FeedbackRepository.GetByIdAsync(request.feedbackId);
        if (feedback == null)
        {
            return new ResponseModel(HttpStatusCode.NotFound, "Feedback not found");
        }
        
        feedback.DeletedAt = DateTime.Now;
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            _unitOfWork.FeedbackRepository.Update(feedback);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            await _unitOfWork.CommitTransactionAsync();
            return new ResponseModel(HttpStatusCode.OK, "Feedback has been deleted.");
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return new ResponseModel(HttpStatusCode.BadRequest, e.Message);
        }
    }
}
