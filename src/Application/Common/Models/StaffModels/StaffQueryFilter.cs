namespace Application.Common.Models.StaffModels;

public class StaffQueryFilter : IRequest<ResponseModel>
{
    public string? Search {get; set;}
    public string? Sort {get; set;}
    public string? Direction {get; set;}
    public int? PageSize {get; set;}
    public int? PageNumber {get; set;}

    public void ApplyDefault()
    {
        if (string.IsNullOrWhiteSpace(Sort))
        {
            Sort = "name";
        }
        
        if (string.IsNullOrWhiteSpace(Direction))
        {
            Direction = "asc";
        }
    }
}