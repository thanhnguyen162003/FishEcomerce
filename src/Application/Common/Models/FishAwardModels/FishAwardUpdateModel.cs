namespace Application.Common.Models.FishAwardModels;

public class FishAwardUpdateModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime AwardDate { get; set; }

}