namespace Application.Common.Models.CustomerModels;

public class CustomerUpdateModel
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? Gender { get; set; }
}