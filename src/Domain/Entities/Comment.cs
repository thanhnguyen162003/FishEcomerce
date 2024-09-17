using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;


public partial class Comment
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public Guid? Blogid { get; set; }

    public Guid? Customerid { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Customer? Customer { get; set; }
}
