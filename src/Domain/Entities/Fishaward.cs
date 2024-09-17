using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Fishaward
{
    public Guid Id { get; set; }

    public Guid? Fishproductid { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual Fishproduct? Fishproduct { get; set; }
}
