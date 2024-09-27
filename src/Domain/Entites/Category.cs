namespace Domain.Entites;

public class Category : BaseEntity
{
    public Guid Id { get; set; }

    public string? TankType { get; set; }

    public string? Level { get; set; }
    
    public virtual ICollection<Tank> Tanks { get; set; } = new List<Tank>();
}