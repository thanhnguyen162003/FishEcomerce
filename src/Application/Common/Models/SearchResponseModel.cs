using Application.Common.Models.BlogModel;
using Application.Common.Models.ProductModels;

namespace Application.Common.Models;

public class SearchResponseModel<T> where T : class
{
    public IEnumerable<T> List { get; init; }
    public bool HasMore { get; init; }

    public SearchResponseModel(IEnumerable<T> list)
    {
        List = list;
        HasMore = list.Count() > 5;
    }
}