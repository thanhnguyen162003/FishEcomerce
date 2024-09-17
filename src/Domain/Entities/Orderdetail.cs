using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Orderdetail
{
    public Guid Id { get; set; }

    public Guid? Orderid { get; set; }

    public Guid? Productid { get; set; }

    public int? Quantity { get; set; }

    public decimal? Unitprice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
