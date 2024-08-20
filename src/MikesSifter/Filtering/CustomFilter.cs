using System.Linq.Expressions;

namespace MikesSifter.Filtering;

internal record CustomFilter(
    FilteringOperators FilteringOperator, 
    Func<object?, Expression> ObtainFilterExpression,
    Func<string?, object?>? FilterValueConverter);