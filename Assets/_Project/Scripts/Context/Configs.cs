using DevNote.Modules.Levels;
using UnityEngine;

namespace DevNote
{
    public static class Configs
    {
        public static LevelsConfig Levels => Resources.Load<LevelsConfig>("[Levels]");

    }
}