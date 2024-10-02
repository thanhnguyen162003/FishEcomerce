namespace Application.Common.Models.FishModels;

public class FishUpdateRequestModel
{
    public Guid? BreedId { get; set; }

    public decimal? Size { get; set; }

    public int? Age { get; set; }

    public string? Origin { get; set; }

    public bool Sex { get; set; }

    public decimal? FoodAmount { get; set; }

    public decimal? Weight { get; set; }

    public string? Health { get; set; }

    //public virtual ICollection<FishAward> Awards { get; set; } = new List<FishAward>();
}