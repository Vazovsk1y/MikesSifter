using System.Text.Json;
using MikesSifter.Filtering;
using MikesSifter.UnitTests.Models;

namespace MikesSifter.UnitTests.Infrastructure;

public class EntitySifterConfiguration : IMikesSifterEntityConfiguration<Entity>
{
    public void Configure(MikesSifterEntityBuilder<Entity> builder)
    {
        builder
            .Property(e => e.PropertyWithDisabledFiltering)
            .EnableSorting();

        builder
            .Property(e => e.PropertyWithDisabledSorting)
            .EnableFiltering();

        builder
            .Property(e => e.Uint)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.String)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Bool)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.NullableInt32)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.DateTimeOffset)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.ComplexType)
            .EnableFiltering()
            .HasCustomFilter(FilteringOperator.Equal, value =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(value, $"{nameof(Filter)}.{nameof(Filter.Value)}");
                var parameter = JsonSerializer.Deserialize<ComplexType>(value);
                ArgumentNullException.ThrowIfNull(parameter);
                
                return e => e.ComplexType == parameter;
            });

        builder
            .Property(e => e.ComplexType.Title)
            .EnableSorting()
            .EnableFiltering()
            .HasAlias("ComplexType_title");

        builder
            .Property(e => e.ComplexType.Value)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.RelatedCollection.Count)
            .EnableSorting()
            .EnableFiltering()
            .HasAlias("RelatedCollectionCount");

        builder
            .Property(e => e.RelatedCollection)
            .EnableFiltering()
            .HasCustomFilter(FilteringOperator.Contains, value =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(value, $"{nameof(Filter)}.{nameof(Filter.Value)}");
                var parameter = JsonSerializer.Deserialize<ComplexType>(value);
                ArgumentNullException.ThrowIfNull(parameter);
                
                return e => e.RelatedCollection.Contains(parameter);
            });
    }
}