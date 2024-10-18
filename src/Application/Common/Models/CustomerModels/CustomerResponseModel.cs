namespace Application.Common.Models.CustomerModels;

public class CustomerResponseModel
{
    public Guid Id { get; set; }
    
    public string? Username { get; set; }
    
    public string? Name { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public int? LoyaltyPoints { get; set; }

    public DateOnly? RegistrationDate { get; set; }
}