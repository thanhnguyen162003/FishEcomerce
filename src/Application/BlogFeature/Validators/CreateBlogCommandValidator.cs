using Application.Common.Models.BlogModel;
using Microsoft.AspNetCore.Http;

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

        RuleFor(x => x.Thumbnail)
            .NotNull().WithMessage("Thumbnail is required.")
            .Must(IsValidFile).WithMessage("Thumbnail must be a valid file.");
    }
    
    private bool IsValidFile(IFormFile file)
    {
        var validTypes = new[] { "image/jpeg", "image/png" };
        return file.Length > 0 && validTypes.Contains(file.ContentType);
    }
}