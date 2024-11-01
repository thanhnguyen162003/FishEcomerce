using Application.Common.Models.ImageModels;
using Microsoft.AspNetCore.Http;

namespace Application.Images.Commands.UploadBlogImage;

public class UploadBlogImageCommandValidator : AbstractValidator<ImageUploadRequestModel>
{
    private readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png"];
    
    public UploadBlogImageCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File is required.");
        
        RuleFor(x => x.File.FileName)
            .Must(HasAllowedExtension)
            .WithMessage("File extension is not valid.");
        
        RuleFor(x => x.File.Length)
            .GreaterThan(0)
            .WithMessage("File cannot be empty");
    }
    
    private bool HasAllowedExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLower();
        return AllowedExtensions.Contains(extension);
    }
}