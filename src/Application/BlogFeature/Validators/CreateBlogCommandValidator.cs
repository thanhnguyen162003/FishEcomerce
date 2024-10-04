using Application.Common.Models.BlogModel;

namespace Application.Blog.Validators;

public class CreateBlogCommandValidator : AbstractValidator<BlogCreateRequestModel>
{
    public CreateBlogCommandValidator()
    {
        RuleFor(x => x.Title)
        .MinimumLength(3).WithMessage("Title must be at least 3 characters long")
        .MaximumLength(255).WithMessage("Title must be at most 255 characters long")
        .NotEmpty().WithMessage("Title is required");

        RuleFor(x => x.Content)
        .MinimumLength(10).WithMessage("Content must be at least 10 characters long")
        .NotEmpty().WithMessage("Content is required");

        RuleFor(x => x.ContentHtml)
        .MinimumLength(10).WithMessage("Content Html must be at least 10 characters long")
        .NotEmpty().WithMessage("Content Html is required");
    }
}