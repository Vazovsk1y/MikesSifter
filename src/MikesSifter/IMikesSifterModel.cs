using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter;

public interface IMikesSifterModel
{
    FilteringOptions? GetFilteringOptions();

    SortingOptions? GetSortingOptions();

    PagingOptions? GetPagingOptions();
}