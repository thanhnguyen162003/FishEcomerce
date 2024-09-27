namespace Domain.Entites;

public class Comment : BaseEntity
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public Guid? BlogId { get; set; }

    public Guid? CustomerId { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Customer? Customer { get; set; }
}