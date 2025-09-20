using System.Collections.Generic;
using DevNote;
using Gamebox;


public partial class GameState // Data
{
    public static ReactiveValue<bool> NoAdsPurchased { get; private set; }
    public static LevelsState Levels { get; private set; }
    public static ItemsState Items { get; private set; }
    public static int LastPlayLocationIndex { get; set; }


}


public partial class GameState : GameStateParser // Parsing
{
    private const string NO_ADS_PURCHASED_KEY = "noAdsPurchased";
    private const string LEVELS_KEY = "levels";
    private const string LAST_PLAY_LOCATION_INDEX = "lastPlayLocationIndex";
    private const string ITEMS_KEY = "items";


    public override void Parse(Dictionary<string, string> data)
    {
        NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "false")));
        Levels = new(data.GetValueOrDefault(LEVELS_KEY, string.Empty));
        LastPlayLocationIndex = int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION_INDEX, "0"));
        Items = new(data.GetValueOrDefault(ITEMS_KEY, string.Empty));
    }

    public override Dictionary<string, string> ToDictionary() => new()
    {
        { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() },
        { LEVELS_KEY, Levels.ToString() },
        { LAST_PLAY_LOCATION_INDEX, LastPlayLocationIndex.ToString() },
        { ITEMS_KEY, Items.ToString() }
    };


    public override bool TransferParsingAvailable => false;
    public override Dictionary<string, string> TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}