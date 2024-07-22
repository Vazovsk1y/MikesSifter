using MikesSifter.Filtering;
using MikesSifter.Paging;
using MikesSifter.Sorting;

namespace MikesSifter;

/// <summary>
/// Interface for a sifter model that provides filtering, paging, and sorting options.
/// </summary>
public interface IMikesSifterModel
{
    /// <summary>
    /// Gets the filtering options from the model.
    /// </summary>
    /// <returns>The filtering options.</returns>
    FilteringOptions? GetFilteringOptions();

    /// <summary>
    /// Gets the sorting options from the model.
    /// </summary>
    /// <returns>The sorting options.</returns>
    SortingOptions? GetSortingOptions();

    /// <summary>
    /// Gets the paging options from the model.
    /// </summary>
    /// <returns>The paging options.</returns>
    PagingOptions? GetPagingOptions();
}