using System.Collections.Generic;
using DevNote;
using Gamebox;
using UnityEngine;


public partial class GameState // Data
{
    public static ReactiveValue<bool> NoAdsPurchased => DevNote.IGameState.NoAdsPurchased;
    public static LevelsState Levels => Gamebox.IGameState.Levels;
    public static ItemsState Items => Gamebox.IGameState.Items;
    public static ReactiveValue<int> LastPlayLocationIndex => Gamebox.IGameState.LastPlayLocationIndex;
}


public partial class GameState : MonoBehaviour, DevNote.IGameState, Gamebox.IGameState // Parsing
{
    int DevNote.IGameState.Version => 1;


    private const string NO_ADS_PURCHASED_KEY = "noAds";
    private const string LEVELS_KEY = "levels";
    private const string LAST_PLAY_LOCATION_INDEX = "lastLocIndex";
    private const string ITEMS_KEY = "items";


    void DevNote.IGameState.Parse(Dictionary<string, string> data)
    {
        DevNote.IGameState.NoAdsPurchased = new(bool.Parse(data.GetValueOrDefault(NO_ADS_PURCHASED_KEY, "False")));
        Gamebox.IGameState.Levels = new(data.GetValueOrDefault(LEVELS_KEY, string.Empty));
        Gamebox.IGameState.LastPlayLocationIndex = new(int.Parse(data.GetValueOrDefault(LAST_PLAY_LOCATION_INDEX, "0")));
        Gamebox.IGameState.Items = new(data.GetValueOrDefault(ITEMS_KEY, string.Empty));
        
    }

    Dictionary<string, string> DevNote.IGameState.ToDictionary() => new()
    {
        { NO_ADS_PURCHASED_KEY, NoAdsPurchased.ToString() },
        { LEVELS_KEY, Levels.ToString() },
        { LAST_PLAY_LOCATION_INDEX, LastPlayLocationIndex.ToString() },
        { ITEMS_KEY, Items.ToString() }
    };


    bool DevNote.IGameState.TransferParsingAvailable => false;
    Dictionary<string, string> DevNote.IGameState.TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}