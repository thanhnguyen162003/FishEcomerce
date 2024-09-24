using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishProduct
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Size { get; set; }

    public string? Age { get; set; }

    public string? Origin { get; set; }

    public string? Sex { get; set; }

    public string? FoodAmount { get; set; }

    public decimal? Weight { get; set; }

    public string? FishType { get; set; }

    public string? Health { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Breed> Breeds { get; set; } = new List<Breed>();

    public virtual ICollection<FishAward> FishAwards { get; set; } = new List<FishAward>();

    public virtual Product? Product { get; set; }
}
