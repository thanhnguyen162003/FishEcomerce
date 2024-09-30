using Application.Common.Models.TankModels;

namespace Application.Common.Models.BreedModels;

public class BreedResponseModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }
}