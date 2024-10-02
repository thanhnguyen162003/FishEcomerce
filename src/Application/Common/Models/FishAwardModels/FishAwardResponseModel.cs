namespace Application.Common.Models.FishAwardModels;

public class FishAwardResponseModel
{
    public Guid Id { get; set; }

    public Guid? FishId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateOnly AwardDate { get; set; }

    public string? Image { get; set; }

}