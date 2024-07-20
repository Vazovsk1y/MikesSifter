namespace MikesSifter.WebApi.Infrastructure;

public class ApplicationSifter : MikesSifter
{
    protected override void Configure(MikesSifterBuilder builder)
    {
        builder.ApplyConfiguration<UserSifterConfiguration>();
    }
}