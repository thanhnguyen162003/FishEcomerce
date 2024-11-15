namespace Application.Common.Models.BlogModel;

public class BlogResponseModel
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    // public string? ContentHtml { get; set; }

    public string? Thumbnail { get; set; }
    
    public string? StaffName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
}