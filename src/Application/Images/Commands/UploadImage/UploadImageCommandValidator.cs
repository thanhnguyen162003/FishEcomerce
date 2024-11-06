using Application.Common.Models.ImageModels;
using Microsoft.AspNetCore.Http;

namespace Application.Images.Commands.UploadImage;

public class UploadImageCommandValidator : AbstractValidator<ImageUploadRequestModel>
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
    
    public UploadImageCommandValidator()
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
        return _allowedExtensions.Contains(extension);
    }
}