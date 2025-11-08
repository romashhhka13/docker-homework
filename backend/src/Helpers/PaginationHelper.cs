using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ShpaginApp.Helpers
{
  namespace Pagination
  {
    public record PaginationRequest
    {
      [Range(1, uint.MaxValue)]
      public uint? Page { get; init; }

      [Range(1, uint.MaxValue)]
      public uint? PageSize { get; init; }
    };

    public record PaginationResponse<T>(
      int Page,
      int PageSize,
      int TotalCount,
      int TotalPages,
      bool HasPreviousPage,
      bool HasNextPage,
      IReadOnlyList<T> Items
    );

    public class PaginatedResult<T>
    {
      public int Page { get; }
      public int PageSize { get; }
      public int TotalCount { get; }
      public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
      public bool HasPreviousPage => Page > 1;
      public bool HasNextPage => Page < TotalPages;
      public IEnumerable<T> Items { get; set; }

      private PaginatedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
      {
        Items = [.. items];
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
      }

      public static async Task<PaginatedResult<TItem>> CreateAsync<TItem>(
        IQueryable<TItem> source,
        PaginationRequest request,
        CancellationToken cancellationToken = default)
        where TItem : class
      {
        var totalCount = await source.CountAsync(cancellationToken);
        var items = await source
          .AsNoTracking()
          .Skip((int)((request.Page - 1) * request.PageSize)!)
          .Take((int)request.PageSize!)
          .ToListAsync(cancellationToken);

        return new PaginatedResult<TItem>(items, totalCount, (int)request.Page!, (int)request.PageSize!);
      }

      public PaginatedResult<TTarget> MapItems<TTarget>(Func<T, TTarget> mapper)
      {
        return new PaginatedResult<TTarget>([.. Items.Select(mapper)], TotalCount, Page, PageSize);
      }
    }
  }
}
