using System.Linq.Expressions;
using System.Reflection;
using MikesSifter.Filtering;

namespace MikesSifter;

internal record MikesSifterPropertyConfiguration(
    PropertyInfo PropertyInfo,
    string PropertyAlias, 
    string TargetPropertyPath, 
    IReadOnlyDictionary<FilteringOperators, Func<string?, Expression>> CustomFilters,
    bool IsFilterable,
    bool IsSortable);