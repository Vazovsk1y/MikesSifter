using MikesSifter.Filtering;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Filtering;

public record FilteringTestCase(FilteringOptions FilteringOptions, IEnumerable<Entity> Expected);