﻿using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class FishTankCategory
{
    public Guid Id { get; set; }

    public string? FishTankCategoryType { get; set; }

    public string? FishTankLevel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<FishTank> FishTankFishTankCategories { get; set; } = new List<FishTank>();
}
