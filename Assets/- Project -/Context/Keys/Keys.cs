using System.Collections.Generic;


public enum ContentKey
{
    StartInterstitial = 0,

    UnlockBooster1 = 1,
    UnlockBooster2 = 2,
    UnlockBooster3 = 3,
    UnlockBooster4 = 5,

    RateUs = 6,
    UnlockCards = 7,
    UnlockShop = 8,
}


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
    Gems = 3,

    Booster1 = 10,
    Booster2 = 11,
    Booster3 = 12,

    CommonBox = 4, RareBox = 5, EpicBox = 6,
    RareCardBox = 7, EpicCardBox = 8, LegendaryCardBox = 9,

}

public static class ItemKeyExtension
{
    private static readonly List<ItemKey> BOOSTERS = new List<ItemKey>() 
    { 
        ItemKey.Booster1, ItemKey.Booster2, ItemKey.Booster3 
    };

    private static readonly List<ItemKey> BOXES = new List<ItemKey>()
    {
        ItemKey.CommonBox, ItemKey.RareBox, ItemKey.RareBox,
        ItemKey.RareCardBox, ItemKey.EpicCardBox, ItemKey.LegendaryCardBox,
    };

    private static readonly List<ItemKey> BOXES_WITH_SINGLE_CARD = new List<ItemKey>()
    {
        ItemKey.RareCardBox, ItemKey.EpicCardBox, ItemKey.LegendaryCardBox,
    };


    public static bool IsBooster(this ItemKey itemKey) => BOOSTERS.Contains(itemKey);
    public static bool IsBox(this ItemKey itemKey) => BOXES.Contains(itemKey);

    public static bool IsBoxWithSingleCard(this ItemKey itemKey) => BOXES_WITH_SINGLE_CARD.Contains(itemKey);


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
    Gems1 = 1, Gems2 = 2, Gems3 = 3, Gems4 = 4, Gems5 = 5, Gems6 = 6
}

public enum RemoteKey
{
    Test = 0,

}
