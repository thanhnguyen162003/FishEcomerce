namespace Domain.Entites;

public class Tank
{
    public Guid Id { get; set; }
    
    public Guid? ProductId { get; set; }

    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }
    
    public virtual ICollection<TankCategory> TankCategories { get; set; } = new List<TankCategory>();

    public virtual Product? Product { get; set; }

}