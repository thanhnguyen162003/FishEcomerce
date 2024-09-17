using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Product
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Categoryid { get; set; }

    public Guid? Supplierid { get; set; }

    public int? Stockquantity { get; set; }

    public decimal? Price { get; set; }

    public int? Productspecificationid { get; set; }

    public int? Sold { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Fishproduct> Fishproducts { get; set; } = new List<Fishproduct>();

    public virtual ICollection<Fishtank> Fishtanks { get; set; } = new List<Fishtank>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual Supplier? Supplier { get; set; }
}
