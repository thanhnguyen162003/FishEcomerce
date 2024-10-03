namespace Domain.Entites;

public class Image : BaseEntity
{
    public Guid Id { get; set; }
    
    public string? PublicId { get; set; }
    
    public string? Link { get; set; }

    public Guid? ProductId { get; set; }

    public Guid? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Product? Product { get; set; }
    
}