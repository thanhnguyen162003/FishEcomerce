using System;
using System.Collections.Generic;

namespace FishEcomerce.Entities;

public partial class Blog
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Contenthtml { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public Guid? Supplierid { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Supplier? Supplier { get; set; }
}
