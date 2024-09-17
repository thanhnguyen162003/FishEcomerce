using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Fishtank
{
    public Guid Id { get; set; }

    public Guid? Productid { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Informationdetail { get; set; }

    public string? Size { get; set; }

    public string? Sizeinformation { get; set; }

    public string? Glasstype { get; set; }

    public virtual ICollection<Fishtankcategory> Fishtankcategories { get; set; } = new List<Fishtankcategory>();

    public virtual Product? Product { get; set; }
}
