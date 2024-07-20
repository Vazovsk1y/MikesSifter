using System.Linq.Expressions;
using MikesSifter.Filtering;

namespace MikesSifter;

internal record MikesSifterPropertyConfiguration(
    string PropertyAlias, 
    string PropertyFullName, 
    Dictionary<FilteringOperators, Func<string?, Expression>> CustomFilters,
    bool IsFilterable,
    bool IsSortable);