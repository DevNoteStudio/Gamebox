public enum CardType
{
    Locked = -1,
    Empty = 0,

    CoinsMultiplier = 1,
    RatingMultiplier = 2,
    Reviver = 3,
    GemRewarder = 4,
    FreeBooster1 = 5,
    FreeBooster2 = 6,
    FreeBooster3 = 7,

}

public enum ItemKey
{
    Coins = 0,
    Stars = 1,
    LocationsUnlocked = 2,
    Gems = 3,

    Booster = 10,

    CommonBox = 4, RareBox = 5, EpicBox = 6,
    RareCard = 7, EpicCard = 8, LegendaryCard = 9,

}

public static class ItemKeyExtension
{
    public static bool IsBooster(this ItemKey itemKey) => itemKey == ItemKey.Booster;
}



public enum EnvironmentKey
{
    Test = 0,
    YandexGames = 1,
    GamePush = 2,
}

public enum AdKey
{
    Default = 0,
    LevelRevive = 1,
    VictoryRoulette = 2,
    LevelStartInterstitial = 3,

}

public enum TableKey
{
    Localization = 0,
    GameboxLocalization = 1,
    Leagues = 2,
    Shop = 3,
    Cards = 4,

}

public enum LeaderboardKey
{
    Main = 0,
}

public enum ProductKey
{
    NoAds = 0,
    Gems1 = 1, 
    Gems2 = 2, 
    Gems3 = 3, 
    Gems4 = 4, 
    Gems5 = 5, 
    Gems6 = 6,

}

public enum RemoteKey
{
    Test = 0,

}
