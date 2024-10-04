using Newtonsoft.Json;

namespace Application.Common.Models;

public class PaginationOptions
{
    public int DefaultPageSize { get; set; } = 10;
    public int DefaultPageNumber { get; set; } = 1;
}