using Application.Common.Models.FeedbackModels;

namespace Application.Feedbacks.Commands.CreateFeedback;

public class CreateFeedbackCommandValidator : AbstractValidator<FeedbackCreateModel>
{
    public CreateFeedbackCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required");
        
        RuleFor(x=>x.Content)
            .NotEmpty().WithMessage("Content is required");
        
        RuleFor(x=>x.Rate)
            .NotEmpty().WithMessage("Rate is required");
    }
}