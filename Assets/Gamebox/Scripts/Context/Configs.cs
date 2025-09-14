using DevNote.Gamebox;
using UnityEngine;

namespace DevNote
{
    public static class Configs
    {
        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("[Gamebox]");


    }
}