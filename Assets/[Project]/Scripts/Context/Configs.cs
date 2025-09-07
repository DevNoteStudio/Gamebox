using DevNote.Gamebox;
using UnityEngine;

namespace DevNote
{
    public static class Configs
    {
        public static LevelUpConfig LevelUp => Resources.Load<LevelUpConfig>("[LevelUp]");

        public static GameboxConfig Gamebox => Resources.Load<GameboxConfig>("[Gamebox]");


    }
}