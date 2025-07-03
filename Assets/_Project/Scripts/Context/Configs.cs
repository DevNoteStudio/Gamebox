using DevNote.Modules.Levels;
using UnityEngine;

namespace DevNote
{
    public static class Configs
    {
        public static LocationsConfig Locations = Resources.Load<LocationsConfig>("Locations");
        public static LevelsUIConfig LevelsUI = Resources.Load<LevelsUIConfig>("LevelsUI");
    }
}