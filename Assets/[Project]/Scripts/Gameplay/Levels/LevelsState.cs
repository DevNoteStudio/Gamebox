using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote.Gamebox
{
    public class LevelsState
    {
        private struct LocationLevelStars
        {
            public List<int> levelStarsAmounts;
        }

        public event Action OnChanged;

        public int TotalStars { get; private set; }
        public int LevelIndex { get; private set; } = 0;
        public int LocationIndex { get; private set; } = 0;


        private List<LocationLevelStars> _locationsLevelList;


        private const char SECTION_SEPARATOR = ':';
        private const char VALUE_SEPARATOR = ',';
        private const char CORTEGE_SEPARATOR = '_';



        public LevelsState(string data)
        {
            var config = Configs.LevelUp;
            _locationsLevelList = new();

            for (int locationIndex = 0; locationIndex < config.LocationsAmount; locationIndex++)
            {
                _locationsLevelList.Add(new LocationLevelStars { levelStarsAmounts = new() });

                for (int levelIndex = 0; levelIndex < config.GetLevelsAmount(locationIndex); levelIndex++)
                    _locationsLevelList[locationIndex].levelStarsAmounts.Add(0);
            }

            if (data != string.Empty)
            {
                string[] splitData = data.Split(SECTION_SEPARATOR);
                string currentLocationLevelData = splitData[0];
                string completedLevelsData = splitData[1];

                int maxLocationIndex = config.LocationsAmount - 1;
                int currentLocationIndex = int.Parse(currentLocationLevelData.Split(VALUE_SEPARATOR)[0]);
                LocationIndex = Mathf.Min(currentLocationIndex, maxLocationIndex);

                int maxLevelIndex = config.GetLevelsAmount(LocationIndex) - 1;
                int currentLevelIndex = int.Parse(currentLocationLevelData.Split(VALUE_SEPARATOR)[1]);
                LevelIndex = Mathf.Min(currentLevelIndex, maxLevelIndex);

                if (completedLevelsData != string.Empty)
                {
                    string[] levelDataList = completedLevelsData.Split(VALUE_SEPARATOR);

                    foreach (string levelData in levelDataList)
                    {
                        string[] splitLevelData = levelData.Split(CORTEGE_SEPARATOR);

                        int locationIndex = int.Parse(splitLevelData[0]);
                        int levelIndex = int.Parse(splitLevelData[1]);
                        int starsAmount = int.Parse(splitLevelData[2]);

                        if (locationIndex < config.LocationsAmount && levelIndex < config.GetLevelsAmount(locationIndex))
                            _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex] = starsAmount;
                    }
                }   
            }

            TotalStars = CalculateTotalStars();
        }


        public bool LevelIsCompleted(int locationIndex, int levelIndex)
        {
            CheckLevel(locationIndex, levelIndex);
            return _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex] > 0;
        }

        public int GetLevelStars(int locationIndex, int levelIndex)
        {
            CheckLevel(locationIndex, levelIndex);
            return _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];
        }

        public void SetCurrentLevel(int locationIndex, int levelIndex)
        {
            LocationIndex = locationIndex;
            LevelIndex = levelIndex;
        }


        public void CompleteLevel(int locationIndex, int levelIndex, int stars)
        {
            if (stars < 1 || 3 < stars)
                throw new Exception($"Wrong stars amount! Stars must be from 1 to 3. Your value: {stars}");

            CheckLevel(locationIndex, levelIndex);

            int previousStarsAmount = _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];
            if (previousStarsAmount < stars)
            {
                _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex] = stars;
                TotalStars = CalculateTotalStars();
                OnChanged?.Invoke();
            }
        }


        public override string ToString()
        {
            var config = Configs.LevelUp;
            string data = $"{LocationIndex}{VALUE_SEPARATOR}{LevelIndex}{SECTION_SEPARATOR}";

            int locationsAmount = config.LocationsAmount;
            for (int locationIndex = 0; locationIndex < locationsAmount; locationIndex++)
            {
                int levelsAmount = config.GetLevelsAmount(locationIndex);
                for (int levelIndex = 0; levelIndex < levelsAmount; levelIndex++)
                {
                    int levelStarsAmount = _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];

                    if (levelStarsAmount > 0)
                        data += $"{locationIndex}{CORTEGE_SEPARATOR}{levelIndex}{CORTEGE_SEPARATOR}{levelStarsAmount}{VALUE_SEPARATOR}";
                }
            }

            if (data[data.Length - 1] == VALUE_SEPARATOR)
                data = data.Remove(data.Length - 1);

            return data;
        }


        private int CalculateTotalStars()
        {
            int value = 0;

            foreach (var locationLevels in _locationsLevelList)
            {
                foreach (int levelStarsAmount in locationLevels.levelStarsAmounts)
                    value += levelStarsAmount;
            }

            return value;
        }

        private void CheckLevel(int locationIndex, int levelIndex)
        {
            if (locationIndex >= _locationsLevelList.Count)
                throw new Exception($"Wrong location - location index: {locationIndex}, level index: {levelIndex}");

            if (levelIndex >= _locationsLevelList[locationIndex].levelStarsAmounts.Count)
                throw new Exception($"Wrong level - location index: {locationIndex}, level index: {levelIndex}");
        }


    }
}


