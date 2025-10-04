using System.Collections.Generic;
using DevNote;
using UnityEngine;
using IGameState = DevNote.IGameState;


public class GameState : MonoBehaviour, IGameState, Gamebox.IGameState // Parsing
{
    int IGameState.Version => 1;



    void IGameState.Parse(Dictionary<string, string> data)
    {
        IGameState.ParseState(data);
        Gamebox.IGameState.ParseState(data);
        
    }

    Dictionary<string, string> IGameState.ToDictionary() => new()
    {
        IGameState.GetStateDictionary(),
        Gamebox.IGameState.ToDictionary()
    };



    bool IGameState.TransferParsingAvailable => false;
    Dictionary<string, string> IGameState.TransferParse(string data)
    {
        throw new System.NotImplementedException();
    }
}