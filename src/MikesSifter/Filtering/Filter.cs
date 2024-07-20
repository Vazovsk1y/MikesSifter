using System.Text.Json.Serialization;

namespace MikesSifter.Filtering;

public record Filter(
    string PropertyAlias, 
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    FilteringOperators Operator, 
    string? Value);