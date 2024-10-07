using Application.Common.Models.BreedModels;
using Application.Common.Models.FishAwardModels;
using Domain.Entites;

namespace Application.Common.Models.FishModels;

public class FishResponseModel
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public BreedResponseModel Breed { get; set; }

    public string? Size { get; set; }

    public string? Age { get; set; }

    public string? Origin { get; set; }

    public string? Sex { get; set; }

    public string? FoodAmount { get; set; }

    public decimal? Weight { get; set; }

    public string? Health { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    //public virtual Breed? Breed { get; set; }

    //public virtual Product? Product { get; set; }

    public virtual ICollection<FishAwardResponseModel> Awards { get; set; } = new List<FishAwardResponseModel>();

}