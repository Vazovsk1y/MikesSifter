namespace MikesSifter.UnitTests.Infrastructure;

public static class Utils
{
    public static T PickRandom<T>(params T[] values)
    {
        return values.ElementAt(Random.Shared.Next(0, values.Length));
    }
}