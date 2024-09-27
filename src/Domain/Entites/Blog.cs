namespace Domain.Entites;

public class Blog : BaseEntity
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Slug { get; set; }

    public string? Content { get; set; }

    public string? ContentHtml { get; set; }

    public Guid? SupplierId { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    
    public virtual Supplier? Supplier { get; set; }
}