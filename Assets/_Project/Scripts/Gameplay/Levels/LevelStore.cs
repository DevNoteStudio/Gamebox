using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote.LevelUp
{
    public class LevelStore
    {
        private struct LocationLevels
        {
            public List<int> levelStarsAmounts;
        }

        public event Action OnChanged;

        public int TotalStars { get; private set; }
        public int LevelIndex { get; private set; } = 0;
        public int LocationIndex { get; private set; } = 0;


        private List<LocationLevels> _locationsLevelList;


        public LevelStore(string data)
        {
            var levelsConfig = Configs.LevelUp;
            _locationsLevelList = new();

            for (int locationIndex = 0; locationIndex < levelsConfig.LocationDataList.Count; locationIndex++)
            {
                _locationsLevelList.Add(new LocationLevels { levelStarsAmounts = new() });

                for (int levelIndex = 0; levelIndex < levelsConfig.LocationDataList[locationIndex].levels; levelIndex++)
                    _locationsLevelList[locationIndex].levelStarsAmounts.Add(0);
            }

            if (data != string.Empty)
            {
                string[] splitData = data.Split(':');
                string currentLocationLevelData = splitData[0];
                string completedLevelsData = splitData[1];

                int maxLocationIndex = levelsConfig.LocationDataList.Count - 1;
                LocationIndex = Mathf.Min(int.Parse(splitData[0].Split('_')[0]), maxLocationIndex);

                int maxLevelIndex = levelsConfig.LocationDataList[LocationIndex].levels - 1;
                LevelIndex = Mathf.Min(int.Parse(splitData[0].Split('_')[1]), maxLevelIndex);

                if (completedLevelsData != string.Empty)
                {
                    string[] levelDataList = completedLevelsData.Split(',');

                    foreach (string levelData in levelDataList)
                    {
                        string[] splitLevelData = levelData.Split('_');

                        int locationIndex = int.Parse(splitLevelData[0]);
                        int levelIndex = int.Parse(splitLevelData[1]);
                        int starsAmount = int.Parse(splitLevelData[2]);

                        if (locationIndex < _locationsLevelList.Count && levelIndex < _locationsLevelList[locationIndex].levelStarsAmounts.Count)
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
            string data = string.Empty;

            data += $"{LocationIndex}_{LevelIndex}:";

            for (int locationIndex = 0; locationIndex < _locationsLevelList.Count; locationIndex++)
            {
                for (int levelIndex = 0; levelIndex < _locationsLevelList[locationIndex].levelStarsAmounts.Count; levelIndex++)
                {
                    int levelStarsAmount = _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];

                    if (levelStarsAmount > 0)
                        data += $"{locationIndex}_{levelIndex}_{levelStarsAmount},";
                }
            }

            if (data.Length > 0) 
                data = data.Remove(data.Length - 1);

            Debug.Log($"Save {data}");
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
                throw new Exception($"Wrong location: location index {locationIndex}, level index {levelIndex}");

            if (levelIndex >= _locationsLevelList[locationIndex].levelStarsAmounts.Count)
                throw new Exception($"Wrong level: location index {locationIndex}, level index {levelIndex}");
        }


    }
}


