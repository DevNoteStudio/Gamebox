using DevNote;

namespace Gamebox
{
    public class RewardController
    {
        private readonly Viewer<RewardScreenView> rewardScreenViewer;


        public RewardController()
        {
            rewardScreenViewer = new(IConfigs.GetViewPrefab<RewardScreenView>());
        }


        public void ShowRewardScreen(ItemPack rewardItemPack)
             => rewardScreenViewer.ShowFaded(UI.Container).Display(rewardItemPack).AnimateShow();

        public void HideRewardScreen() 
            => rewardScreenViewer.AnimateFadedHide(rewardScreenViewer.View.AnimateHide);



    }
}

