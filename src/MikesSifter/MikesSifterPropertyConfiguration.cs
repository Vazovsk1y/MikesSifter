using System.Reflection;
using MikesSifter.Filtering;

namespace MikesSifter;

internal record MikesSifterPropertyConfiguration(
    PropertyInfo PropertyInfo,
    string PropertyAlias, 
    string TargetPropertyPath, 
    IReadOnlyCollection<CustomFilter> CustomFilters,
    bool IsFilterable,
    bool IsSortable);
