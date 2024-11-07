using Microsoft.AspNetCore.Http;

namespace Application.Common.Models.BlogModel;

public class BlogCreateRequestModel
{
    public string? Title { get; set; }

    public string? Content { get; set; }
    
    public string? ContentHtml { get; set; }
    
    public IFormFile? Thumbnail  { get; set; }
    
}