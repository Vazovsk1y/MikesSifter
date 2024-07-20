namespace MikesSifter.Paging;

/// <summary>
/// Represents paging options including the page index and page size.
/// </summary>
/// <param name="PageIndex">The index of the page to retrieve.</param>
/// <param name="PageSize">The number of items per page.</param>
public record PagingOptions(int PageIndex, int PageSize);