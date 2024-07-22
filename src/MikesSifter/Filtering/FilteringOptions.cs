using System.Text.Json.Serialization;

namespace MikesSifter.Filtering;

/// <summary>
/// Represents filtering options including the logic and collection of filters to apply.
/// </summary>
/// <param name="Logic">The logic to use when combining multiple filters. Uses <see cref="FilteringLogic"/> enum.</param>
/// <param name="Filters">The collection of filters to apply.</param>
public record FilteringOptions(
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    FilteringLogic Logic, 
    IReadOnlyCollection<Filter> Filters);