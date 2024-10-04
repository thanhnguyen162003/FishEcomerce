namespace Application.Common.Models.BlogModel;

public class BlogResponseModel
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    public string? ContentHtml { get; set; }

    public string? SupplierName { get; set; }
    
}