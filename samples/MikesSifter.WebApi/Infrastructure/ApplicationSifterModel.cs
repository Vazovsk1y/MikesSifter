using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter.WebApi.Infrastructure;

public sealed class ApplicationSifterModel : IMikesSifterModel
{
    public FilteringOptions? FilteringOptions { get; init; }
    
    public SortingOptions? SortingOptions { get; init; }
    
    public PagingOptions? PagingOptions { get; init; }

    public FilteringOptions? GetFilteringOptions() => FilteringOptions;

    public SortingOptions? GetSortingOptions() => SortingOptions;

    public PagingOptions? GetPagingOptions() => PagingOptions;
}