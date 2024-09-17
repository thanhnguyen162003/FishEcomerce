using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Image
{
    public Guid Id { get; set; }

    public string? Cloudlink { get; set; }

    public Guid? Productid { get; set; }

    public Guid? Blogid { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Product? Product { get; set; }
}
