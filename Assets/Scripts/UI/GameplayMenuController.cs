using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IGameplayMenuController
    {
        void ShowMenu();
        void HideMenu();
        void ToMissionBoard();
        void ToMainMenu();
    }
    public class GameplayMenuController : GameplayController,  IGameplayMenuController 
    {
        [Header("Configuration")]
        [SerializeField] private GameObject _menuPopUpObject;

        public void HideMenu() => _menuPopUpObject.SetActive(false);
        public void ShowMenu() => _menuPopUpObject.SetActive(true);
        public void ToMainMenu()
        {
            Debug.LogWarning("Attempted to go to main menu fail");
        }
        public void ToMissionBoard()
        {
            Debug.LogWarning("Attempted to go to mission board fail");
        }

        #region Unity basic
        private void Start() => _menuPopUpObject.SetActive(false);
        #endregion
    }
}


