namespace Domain.Entites;

public class Category : BaseEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    
    public string? Type { get; set; }
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}