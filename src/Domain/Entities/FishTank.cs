using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishTank
{
    public Guid Id { get; set; }

    public string? InformationDetail { get; set; }

    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? ProductId { get; set; }

    public virtual ICollection<FishTankFishTankCategory> FishTankFishTankCategories { get; set; } = new List<FishTankFishTankCategory>();

    public virtual Product? Product { get; set; }
}
