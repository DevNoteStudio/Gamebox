namespace DevNote.Gamebox
{
    public static class StarsCalculator
    {
        private const int MAX_STARS_FOR_LEVEL = 3;


        public static int GetLocationMaxStars(int locationIndex)
            => Configs.Gamebox.GetLocationLevelsAmount(locationIndex) * MAX_STARS_FOR_LEVEL;

        public static int GetLocationCurrentStars(int locationIndex)
        {
            int stars = 0;
            for (int levelIndex = 0; levelIndex < Configs.Gamebox.GetLocationLevelsAmount(locationIndex); levelIndex++)
                stars += GameState.Levels.GetLevelStars(locationIndex, levelIndex);

            return stars;
        }



    }
}

