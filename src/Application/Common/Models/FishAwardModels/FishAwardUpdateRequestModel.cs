﻿namespace Application.Common.Models.FishAwardModels;

public class FishAwardUpdateRequestModel
{

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime AwardDate { get; set; }

    public string? Image { get; set; }

}