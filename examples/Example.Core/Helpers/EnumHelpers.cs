namespace Example.Core.Helpers;

public static class EnumHelpers
{
    public static T GetRandomValue<T>() where T : struct, Enum
    {
        var values = Enum.GetValues<T>();
        var valueIndex = Random.Shared.Next(0, values.Length);
        return values[valueIndex];
    }
}