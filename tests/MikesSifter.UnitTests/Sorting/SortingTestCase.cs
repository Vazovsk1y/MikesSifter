using MikesSifter.Sorting;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Sorting;

public record SortingTestCase(SortingOptions SortingOptions, IEnumerable<Entity> Expected);