namespace MikesSifter.Filtering;

/// <summary>
/// Defines a mechanism for converting a string filter value into a strongly-typed object.
/// </summary>
/// <typeparam name="T">The type of the object that the filter value will be converted to.</typeparam>
public interface IFilterValueConverter<out T>
{
    /// <summary>
    /// Converts the specified string filter value to the target type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="filterValue">The string representation of the filter value to convert.</param>
    T? ConvertFrom(string? filterValue);
}
