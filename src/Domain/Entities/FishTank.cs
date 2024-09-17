using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishTank
{
    public Guid Id { get; set; }

    public Guid? ProductId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? InformationDetail { get; set; }

    public string? Size { get; set; }

    public string? SizeInformation { get; set; }

    public string? GlassType { get; set; }

    public virtual ICollection<FishTankCategory> FishTankCategories { get; set; } = new List<FishTankCategory>();

    public virtual Product? Product { get; set; }
}
