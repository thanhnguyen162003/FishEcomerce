namespace Application.Common.Models.StaffModels;

public class StaffPasswordUpdateModel
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}