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
            .EnableSorting()
            .HasCustomFilters(e =>
            {
                e.WithFilter(FilteringOperators.GreaterThan, filterValue =>
                {
                    ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
                    return i => i.String.Length > filterValue.Length;
                });
            });

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
            .HasCustomFilters(e =>
            {
                e.WithFilter(FilteringOperators.Equal, Converter, filterValue => i => i.ComplexType == filterValue);

                ComplexType? Converter(string? o) => string.IsNullOrWhiteSpace(o) ? null : JsonSerializer.Deserialize<ComplexType>(o);
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
            .HasCustomFilters(e =>
            {
                e.WithFilter(FilteringOperators.Contains, new ComplexTypeFilterValueConverter(), filterValue =>
                {
                    ArgumentNullException.ThrowIfNull(filterValue);
                    return i => i.RelatedCollection.Contains(filterValue);
                });
            });
    }
}