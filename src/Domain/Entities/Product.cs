using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public Guid? SupplierId { get; set; }

    public int? StockQuantity { get; set; }

    public decimal? Price { get; set; }

    public int? ProductSpecificationId { get; set; }

    public int? Sold { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<FishProduct> FishProducts { get; set; } = new List<FishProduct>();

    public virtual ICollection<FishTank> FishTanks { get; set; } = new List<FishTank>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
