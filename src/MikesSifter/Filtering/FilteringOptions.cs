using System.Text.Json.Serialization;

namespace MikesSifter.Filtering;

public record FilteringOptions(
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    FilteringLogic Logic, 
    IReadOnlyCollection<Filter> Filters);