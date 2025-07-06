using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote.Modules.Levels
{
    public class LevelProgress
    {
        private struct LocationLevels
        {
            public List<int> levelStarsAmounts;
        }

        public event Action OnProgressChanged;

        public int TotalStarsAmount { get; private set; }
        public int CurrentLevelIndex { get; private set; } = 0;
        public int CurrentLocationIndex { get; private set; } = 0;


        private List<LocationLevels> _locationsLevelList;


        public LevelProgress(string data)
        {
            Debug.Log(data);

            var levelsConfig = Configs.Levels;
            _locationsLevelList = new();

            for (int locationIndex = 0; locationIndex < levelsConfig.LocationDataList.Count; locationIndex++)
            {
                _locationsLevelList.Add(new LocationLevels { levelStarsAmounts = new() });

                for (int levelIndex = 0; levelIndex < levelsConfig.LocationDataList[locationIndex].levelsAmount; levelIndex++)
                    _locationsLevelList[locationIndex].levelStarsAmounts.Add(0);
            }

            if (data != string.Empty)
            {
                string[] splitData = data.Split(':');
                string currentLocationLevelData = splitData[0];
                string completedLevelsData = splitData[1];

                int maxLocationIndex = levelsConfig.LocationDataList.Count - 1;
                CurrentLocationIndex = Mathf.Min(int.Parse(splitData[0].Split('_')[0]), maxLocationIndex);

                int maxLevelIndex = levelsConfig.LocationDataList[CurrentLocationIndex].levelsAmount - 1;
                CurrentLevelIndex = Mathf.Min(int.Parse(splitData[0].Split('_')[1]), maxLevelIndex);

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

            TotalStarsAmount = CalculateTotalStarsAmount();
        }


        public bool LevelIsCompleted(int locationIndex, int levelIndex)
        {
            CheckLevel(locationIndex, levelIndex);
            return _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex] > 0;
        }

        public int GetLevelStarsAmount(int locationIndex, int levelIndex)
        {
            CheckLevel(locationIndex, levelIndex);
            return _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];
        }

        public void SetCurrentLevel(int locationIndex, int levelIndex)
        {
            CurrentLocationIndex = locationIndex;
            CurrentLevelIndex = levelIndex;
        }


        public void CompleteLevel(int locationIndex, int levelIndex, int starsAmount)
        {
            if (starsAmount < 1 || 3 < starsAmount)
                throw new Exception($"Wrong stars amount! Stars must be from 1 to 3. Your value: {starsAmount}");

            CheckLevel(locationIndex, levelIndex);

            int previousStarsAmount = _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex];
            if (previousStarsAmount < starsAmount)
            {
                _locationsLevelList[locationIndex].levelStarsAmounts[levelIndex] = starsAmount;
                TotalStarsAmount = CalculateTotalStarsAmount();
                OnProgressChanged?.Invoke();
            }
        }


        public override string ToString()
        {
            string data = string.Empty;

            data += $"{CurrentLocationIndex}_{CurrentLevelIndex}:";

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


        private int CalculateTotalStarsAmount()
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


