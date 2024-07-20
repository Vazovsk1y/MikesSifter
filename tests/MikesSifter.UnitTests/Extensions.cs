namespace MikesSifter.UnitTests;

public static class Extensions
{
    public static T PickRandom<T>(params T[] values)
    {
        return values.ElementAt(Random.Shared.Next(0, values.Length));
    }
}