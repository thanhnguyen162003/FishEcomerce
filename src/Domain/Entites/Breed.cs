namespace Domain.Entites;

public class Breed : BaseEntity
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Fish> Fishes { get; set; } = new List<Fish>();
}