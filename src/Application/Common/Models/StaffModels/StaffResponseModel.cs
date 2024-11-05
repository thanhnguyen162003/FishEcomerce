namespace Application.Common.Models.StaffModels;

public class StaffResponseModel
{
    public Guid Id { get; set; }

    public string? Username { get; set; }
    
    public string? FullName { get; set; }
    
    public string? Facebook { get; set; }
    
    public bool? IsAdmin { get; set; }
    
    public DateTime? CreatedAt { get; set; }

}