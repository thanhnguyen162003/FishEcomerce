namespace Application.Common.Models.BlogModel;

public class BlogUpdateRequestModel
{
    public string? Title { get; set; }

    public string? Content { get; set; }
    
    public string? ContentHtml { get; set; }
}