using Gameplay.UI;

namespace Gameplay
{
    class BasicUILogic : IGameplayUILogic
    {
        private readonly IConsoleTabsController consoleTabs;
        private readonly IActionButtonController actionButton;

        public BasicUILogic(IConsoleTabsController consoleTabs, IActionButtonController actionButton)
        {
            this.consoleTabs = consoleTabs;
            this.actionButton = actionButton;
        }
        public void SetDisplayedActionButton(ActionButtonType buttonType) => actionButton.ActionButtonType = buttonType;
        public void UpdateUIDisplay(TabType currentTab, bool canProceed)
        {
            consoleTabs.SetTab(currentTab);

            switch (currentTab)
            {
                case TabType.CONSTRUCT: actionButton.ActionButtonType = ActionButtonType.EXECUTION; break;
                case TabType.RESULT: actionButton.ActionButtonType = (canProceed) ? ActionButtonType.PROCEED : ActionButtonType.STEP_BACK; break;
            }
        }
    }
}