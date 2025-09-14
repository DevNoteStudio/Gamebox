using System;

namespace DevNote.Gamebox
{
    public partial class GameboxConfig
    {
        [Serializable] private struct AdsData
        {
            public int showReviveFromLevel;
            public int showVictoryRouletteFromLevel;
        }


        public bool VictoryRouletteAvailable(int completedLevels) 
            => completedLevels >= _ads.showVictoryRouletteFromLevel;

        public bool ReviveAvailable(int completedLevels)
            => completedLevels >= _ads.showReviveFromLevel - 1;



    }
}

