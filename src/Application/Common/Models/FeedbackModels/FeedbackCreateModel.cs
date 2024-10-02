namespace Application.Common.Models.FeedbackModels;

public class FeedbackCreateModel
{
    public Guid? ProductId { get; set; }
    
    public string? Content { get; set; }
    
    public int? Rate { get; set; }
}