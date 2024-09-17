using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Fishproduct
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public string? Name { get; set; }

    public string? Size { get; set; }

    public string? Age { get; set; }

    public string? Origin { get; set; }

    public string? Sex { get; set; }

    public string? Foodamount { get; set; }

    public decimal? Weight { get; set; }

    public string? Fishtype { get; set; }

    public string? Health { get; set; }

    public DateOnly? Dateofbirth { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Breed> Breeds { get; set; } = new List<Breed>();

    public virtual ICollection<Fishaward> Fishawards { get; set; } = new List<Fishaward>();

    public virtual Product? Product { get; set; }
}
