using DevNote;

namespace Gamebox
{
    public class StartController : IStartHandler
    {
        private readonly MenuController menuController;
        private readonly LevelController levelController;
        private readonly PopupController popupController;

        private const int CURRENT_SAVE_VERSION = 1;


        public StartController(MenuController menuController, LevelController levelController, 
            PopupController popupController)
        {
            this.menuController = menuController;
            this.levelController = levelController;
            this.popupController = popupController;
        }


        void IStartHandler.Start()
        {
            if (DevNote.IGameState.SaveVersion.Value < CURRENT_SAVE_VERSION)
                ConvertRatingAndGetPassedLeagueRewards();


            int locationIndex = IGameState.LastPlayLocationIndex.Value;

            if (IConfigs.Gamebox.MenuAvailable)
                menuController.ShowLocationsScreen(locationIndex);

            else
            {
                int levelIndex = IGameState.Levels.GetLastLevelIndexForPlay(locationIndex);
                levelController.StartLevel(locationIndex, levelIndex);
            }
        }


        private void ConvertRatingAndGetPassedLeagueRewards()
        {
            if (IGameState.Rating.Value != 0)
            {
                IGameState.Rating.Value = IGameState.Rating.Value / 10;

                var currentLeague = IConfigs.Gamebox.GetLeagueType(IGameState.Rating.Value);

                for (int i = 0; i <= (int)currentLeague; i++)
                {
                    var passedLeagueRewards = IConfigs.Gamebox.GetLeagueRewardItems((LeagueType)i);
                    foreach (var rewardItemPack in passedLeagueRewards)
                        IGameState.Items.Add(rewardItemPack.itemKey, rewardItemPack.amount);
                }
            }

            DevNote.IGameState.SaveVersion.Value = CURRENT_SAVE_VERSION;
        }





    }
}


