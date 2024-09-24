using System;
using System.Collections.Generic;

namespace FishEcomerce.Domain.Entities;

public partial class Comment
{
    public Guid Id { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? BlogId { get; set; }

    public Guid? CustomerId { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual Customer? Customer { get; set; }
}
