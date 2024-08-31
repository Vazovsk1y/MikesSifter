using System.Linq.Expressions;

namespace MikesSifter.Filtering;

/// <summary>
/// Represents a builder class for creating custom filters for a given entity type.
/// </summary>
/// <typeparam name="TEntity">The type of the entity for which filters are being built.</typeparam>
public class CustomFiltersBuilder<TEntity>
{
    private readonly Dictionary<FilteringOperator, (Func<object?, Expression> obtainFilterExpression, Func<string?, object?>? filterValueConverter)> _filters = [];

    /// <summary>
    /// Adds a custom filter with the specified filtering operator.
    /// </summary>
    /// <param name="operator">The filtering operator associated with the filter.</param>
    /// <param name="obtainFilterExpression">A function that returns an expression representing the filter logic for the specified operator.</param>
    /// <returns>The current instance of <see cref="CustomFiltersBuilder{TEntity}"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="obtainFilterExpression"/> is null.</exception>
    public CustomFiltersBuilder<TEntity> WithFilter(FilteringOperator @operator, Func<string?, Expression<Func<TEntity, bool>>> obtainFilterExpression)
    {
        ArgumentNullException.ThrowIfNull(obtainFilterExpression);

        _filters[@operator] = (filterValue => obtainFilterExpression((string?)filterValue), null);
        return this;
    }

    /// <summary>
    /// Adds a custom filter with the specified filtering operator and filter value converter.
    /// </summary>
    /// <typeparam name="TFilterValue">The type of the filter value.</typeparam>
    /// <param name="operator">The filtering operator associated with the filter.</param>
    /// <param name="filterValueConverter">An instance of <see cref="IFilterValueConverter{TFilterValue}"/> used to convert the filter value.</param>
    /// <param name="obtainFilterExpression">A function that returns an expression representing the filter logic for the specified operator.</param>
    /// <returns>The current instance of <see cref="CustomFiltersBuilder{TEntity}"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filterValueConverter"/> or <paramref name="obtainFilterExpression"/> is null.
    /// </exception>
    public CustomFiltersBuilder<TEntity> WithFilter<TFilterValue>(
        FilteringOperator @operator,
        IFilterValueConverter<TFilterValue> filterValueConverter,
        Func<TFilterValue?, Expression<Func<TEntity, bool>>> obtainFilterExpression)
    {
        ArgumentNullException.ThrowIfNull(obtainFilterExpression);
        ArgumentNullException.ThrowIfNull(filterValueConverter);

        _filters[@operator] = (filterValue => obtainFilterExpression((TFilterValue?)filterValue), filterValue => filterValueConverter.Convert(filterValue));
        return this;
    }

    /// <summary>
    /// Adds a custom filter with the specified filtering operator and filter value converter function.
    /// </summary>
    /// <typeparam name="TFilterValue">The type of the filter value.</typeparam>
    /// <param name="operator">The filtering operator associated with the filter.</param>
    /// <param name="filterValueConverter">A function used to convert the filter value from a string.</param>
    /// <param name="obtainFilterExpression">A function that returns an expression representing the filter logic for the specified operator.</param>
    /// <returns>The current instance of <see cref="CustomFiltersBuilder{TEntity}"/> for method chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filterValueConverter"/> or <paramref name="obtainFilterExpression"/> is null.
    /// </exception>
    public CustomFiltersBuilder<TEntity> WithFilter<TFilterValue>(
        FilteringOperator @operator,
        Func<string?, TFilterValue?> filterValueConverter,
        Func<TFilterValue?, Expression<Func<TEntity, bool>>> obtainFilterExpression)
    {
        ArgumentNullException.ThrowIfNull(obtainFilterExpression);
        ArgumentNullException.ThrowIfNull(filterValueConverter);

        _filters[@operator] = (filterValue => obtainFilterExpression((TFilterValue?)filterValue), e => filterValueConverter(e));
        return this;
    }

    internal IReadOnlyCollection<CustomFilter> Build()
    {
        return _filters.Select(e => new CustomFilter(e.Key, e.Value.obtainFilterExpression, e.Value.filterValueConverter)).ToList();
    }
}