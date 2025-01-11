using System.Reflection;
using MikesSifter.Filtering;

namespace MikesSifter;

internal record EntityConfiguration(
    Type EntityType, 
    IReadOnlyCollection<PropertyConfiguration> PropertyConfigurations);
    
internal record PropertyConfiguration(
    PropertyInfo PropertyInfo,
    string PropertyAlias, 
    string TargetPropertyPath, 
    IReadOnlyCollection<CustomFilter> CustomFilters,
    bool IsFilterable,
    bool IsSortable);