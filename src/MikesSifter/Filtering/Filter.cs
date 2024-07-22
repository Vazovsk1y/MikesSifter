using System.Text.Json.Serialization;

namespace MikesSifter.Filtering;

/// <summary>
/// Represents a filtering criterion for a specific property.
/// </summary>
/// <param name="PropertyAlias">The alias of the property to be filtered.</param>
/// <param name="Operator">The operator used for filtering. Uses <see cref="FilteringOperators"/> enum.</param>
/// <param name="Value">The value to filter the property by.</param>
public record Filter(
    string PropertyAlias, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    FilteringOperators Operator, 
    string? Value);