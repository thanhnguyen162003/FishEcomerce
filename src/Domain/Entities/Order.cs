using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Order
{
    public Guid Id { get; set; }

    public Guid? Customerid { get; set; }

    public DateTime? Orderdate { get; set; }

    public DateTime? Shippeddate { get; set; }

    public decimal? Totalprice { get; set; }

    public string? Shipaddress { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
}
