namespace MikesSifter.UnitTests.Infrastructure;

public class TestSifter : MikesSifter
{
    protected override void Configure(MikesSifterBuilder builder)
    {
        builder.ApplyConfiguration<EntitySifterConfiguration>();
    }
}