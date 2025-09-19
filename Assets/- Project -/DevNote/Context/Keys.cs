using System.Collections.Generic;


public static partial class EnvironmentKey
{
    public const string YandexGames = nameof(YandexGames);

}

public static class AdKey
{
    public const string LevelRevive = nameof(LevelRevive);
    public const string VictoryRoulette = nameof(VictoryRoulette);
}

public static class TableKey
{

}

public static class LeaderboardKey
{
    public const string Stars = nameof(Stars);
}

public static class ProductKey
{
    public const string NoAds = nameof(NoAds);



    private static readonly List<string> consumableProductKeys = new() 
    { 

    };
    public static bool IsConsumable(this string productKey) => consumableProductKeys.Contains(productKey);
}

