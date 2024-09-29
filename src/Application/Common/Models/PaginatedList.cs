
using System.Drawing.Printing;

namespace Application.Common.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public int PageSize { get; set; }

    public PaginatedList(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        var count =  source.Count();
        var items =  source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}