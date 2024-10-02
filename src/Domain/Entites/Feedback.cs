namespace Domain.Entites;

public class Feedback : BaseEntity
{
    public Guid Id { get; set; } 

    public Guid? ProductId { get; set; }
    
    public Guid? UserId { get; set; }

    public string? Content { get; set; }

    public int? Rate { get; set; }

    public virtual Product? Product { get; set; }
    
    public virtual Customer? User { get; set; }
}