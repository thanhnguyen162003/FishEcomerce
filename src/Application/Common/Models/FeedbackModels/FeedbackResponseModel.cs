namespace Application.Common.Models.FeedbackModels;

public class FeedbackResponseModel
{
    public Guid Id { get; set; } 

    public Guid? ProductId { get; set; }
    
    public Guid? UserId { get; set; }

    public string? Content { get; set; }

    public int? Rate { get; set; }
}