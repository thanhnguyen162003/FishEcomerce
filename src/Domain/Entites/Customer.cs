﻿namespace Domain.Entites;

public class Customer : BaseEntity
{
    public Guid Id { get; set; }
    
    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? LoyaltyPoints { get; set; }

    public DateOnly? RegistrationDate { get; set; }
    
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}