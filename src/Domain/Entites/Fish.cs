namespace Domain.Entites;

public class Fish
{
    public Guid Id { get; set; }
    
    public Guid? ProductId { get; set; }
    
    public Guid? BreedId { get; set; }

    public string? Size { get; set; }

    public string? Age { get; set; }

    public string? Origin { get; set; }

    public string? Sex { get; set; }

    public string? FoodAmount { get; set; }

    public decimal? Weight { get; set; }
    
    public string? Health { get; set; }
    
    public DateOnly? DateOfBirth { get; set; }
    
    public virtual Breed? Breed { get; set; }
    
    public virtual Product? Product { get; set; }

    public virtual ICollection<FishAward> Awards { get; set; } = new List<FishAward>();
}