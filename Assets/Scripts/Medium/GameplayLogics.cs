using Gameplay.UI;

namespace Gameplay
{
    class BasicUILogic : IGameplayUILogic
    {
        private readonly IActionButtonController actionButton;

        public BasicUILogic(IActionButtonController actionButton) => this.actionButton = actionButton;

        #region Aux methods
        public void updateActionButtonSprite(TabType currentTab, bool canProceed)
        {
            switch (currentTab)
            {
                case TabType.CONSTRUCT: actionButton.ActionButtonType = ActionButtonType.EXECUTION; break;
                case TabType.RESULT: actionButton.ActionButtonType = (canProceed) ? ActionButtonType.PROCEED : ActionButtonType.STEP_BACK; break;
            }
        }
        #endregion
    }
}