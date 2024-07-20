using MikesSifter.WebApi.Models;

namespace MikesSifter.WebApi.Infrastructure;

public class ApplicationSifter : MikesSifter
{
    protected override void Configure(MikesSifterBuilder builder)
    {
        builder.Entity<Project>()
            .Property(e => e.Title)
            .EnableFiltering()
            .EnableSorting();
        
        builder.ApplyConfiguration<UserSifterConfiguration>();
    }
}