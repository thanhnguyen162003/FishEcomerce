namespace Domain.Entites;

public class Staff : BaseEntity
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }
    
    public string? Facebook { get; set; }
    
    public bool? IsAdmin { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}