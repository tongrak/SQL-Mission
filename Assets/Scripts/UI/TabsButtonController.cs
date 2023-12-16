using System.Collections;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IConsoleTabsController
    {
        TabType CurrentTab { get; set; }
    }

    public class TabsButtonController : GameplayController, IConsoleTabsController
    {
        //TODO: Visual feedback improvement

        private TabType _selectedTab = TabType.CONSTRUCT;
        public TabType CurrentTab { get => _selectedTab; set => _selectedTab = value; }
    }
}