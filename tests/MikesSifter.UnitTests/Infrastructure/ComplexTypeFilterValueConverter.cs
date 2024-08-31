using System.Text.Json;
using MikesSifter.Filtering;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Infrastructure;

public class ComplexTypeFilterValueConverter : IFilterValueConverter<ComplexType>
{
    public ComplexType? Convert(string? filterValue)
    {
        return string.IsNullOrWhiteSpace(filterValue) ? null : JsonSerializer.Deserialize<ComplexType>(filterValue);
    }
}