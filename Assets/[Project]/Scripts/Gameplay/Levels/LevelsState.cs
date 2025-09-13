using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DevNote.Gamebox
{
    public class LevelsState
    {
        private class LevelData
        {
            public int index;
            public int stars;
        }

        private class LocationData
        {
            public int index;
            public List<LevelData> levels;
        }

        private List<LocationData> _locations;


        public LevelsState(string data)
        {
            Debug.Log(data);
            _locations = new List<LocationData>();

            if (data != string.Empty)
            {
                string[] splitedLevelData = data.Split(S.S2);

                foreach (var levelData in splitedLevelData)
                {
                    string[] levelDataValues = levelData.Split(S.S1);

                    int locationIndex = int.Parse(levelDataValues[0]);
                    int levelIndex = int.Parse(levelDataValues[1]);
                    int stars = int.Parse(levelDataValues[2]);

                    GetOrCreateLevelData(locationIndex, levelIndex).stars = stars;
                }
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var levelDataList = new List<string>();

            foreach (var locationData in _locations)
            {
                foreach (var levelData in locationData.levels)
                {
                    if (levelData.stars > 0)
                        levelDataList.Add($"{locationData.index}{S.S1}{levelData.index}{S.S1}{levelData.stars}");
                }
            }
            builder.AppendJoin(S.S2, levelDataList);

            return builder.ToString();
        }

        public int GetLevelStars(int locationIndex, int levelIndex)
            => GetOrCreateLevelData(locationIndex, levelIndex).stars;

        public void SetLevelStars(int locationIndex, int levelIndex, int stars)
            => GetOrCreateLevelData(locationIndex, levelIndex).stars = stars;


        private LocationData GetOrCreateLocationData(int locationIndex)
        {
            var locationData = _locations.Find(data => data.index == locationIndex);

            if (locationData == null)
            {
                locationData = new LocationData { index = locationIndex, levels = new() };
                _locations.Add(locationData);
            }

            return locationData;
        }

        private LevelData GetOrCreateLevelData(int locationIndex, int levelIndex)
        {
            var locationData = GetOrCreateLocationData(locationIndex);
            var levelData = locationData.levels.Find(data => data.index == levelIndex);

            if (levelData == null)
            {
                levelData = new LevelData { index = levelIndex, stars = 0 };
                locationData.levels.Add(levelData);
            }

            return levelData;
        }

    }
}


