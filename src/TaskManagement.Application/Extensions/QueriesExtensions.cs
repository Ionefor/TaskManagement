using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Abstractions.Models;

namespace TaskManagement.Application.Extensions;

public static class QueriesExtensions
{
    public static async Task<PageList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        
        var items = await source.
            Skip((page - 1) * pageSize).
            Take(pageSize).
            ToListAsync(cancellationToken);

        return new PageList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}