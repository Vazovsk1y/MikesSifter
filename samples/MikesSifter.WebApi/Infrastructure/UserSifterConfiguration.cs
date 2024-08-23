using MikesSifter.Filtering;
using MikesSifter.WebApi.Models;

namespace MikesSifter.WebApi.Infrastructure;

public class UserSifterConfiguration : IMikesSifterEntityConfiguration<User>
{
    public void Configure(MikesSifterEntityBuilder<User> builder)
    {
        builder
            .Property(e => e.FullName)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Gender)
            .EnableSorting()
            .EnableFiltering();

        builder
            .Property(e => e.BirthDate)
            .EnableFiltering()
            .EnableSorting();

        builder
            .Property(e => e.Projects)
            .EnableFiltering()
            .HasCustomFilter(FilteringOperator.Contains, filterValue =>
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(filterValue);
                return u => u.Projects.Any(e => e.Id == Guid.Parse(filterValue));
            });

        builder
            .Property(e => e.Passport.Number)
            .EnableFiltering()
            .EnableSorting()
            .HasAlias("user_passportNumber");
    }
}