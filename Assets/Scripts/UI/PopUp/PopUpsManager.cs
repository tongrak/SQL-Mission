using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.PopUp
{
    public interface AbleToSoftLoad
    {
        void DisplaySoftLoading();
    }
    public interface IPopUpsManager: AbleToSoftLoad
    {
        //void DisplayGameplayMenu();
        void DisplayCompletionMenu(bool canContinueMission);
    }
    public class PopUpsManager : GameplayController, IPopUpsManager
    {
        [Header("Gameobjects")]
        [SerializeField] private GameObject _gameplayMenuGO;
        [SerializeField] private GameObject _completionMenuGO;
        [SerializeField] private GameObject _contiueButtonGO;
        [SerializeField] protected GameObject _softLoadingGO;
        [SerializeField] protected GameObject _popUpShawdowGO;

        [Header("Continue button sprite")]
        [SerializeField] private Sprite _activeContinueButton;
        [SerializeField] private Sprite _inactiveContinueButton;

        #region AUX methods
        protected virtual void initAllPopUp()
        {
            //Hide all popUps
            _gameplayMenuGO.SetActive(false);
            _completionMenuGO.SetActive(false);
            _softLoadingGO.SetActive(false);
            // Make shadow appear & unclickable
            _popUpShawdowGO.SetActive(true);
            _popUpShawdowGO.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }
        #endregion

        #region interface Functions
        public void HideMenuPopUp()
        {
            _gameplayMenuGO.SetActive(false);
            _popUpShawdowGO.SetActive(false);
        }
        #endregion

        public void DisplayCompletionMenu(bool canContinueMission)
        {
            this.initAllPopUp();
            _completionMenuGO.SetActive(true);
            //set continue button
            _contiueButtonGO.GetComponent<UnityEngine.UI.Image>().sprite = canContinueMission ? _activeContinueButton : _inactiveContinueButton;
            _contiueButtonGO.GetComponent<UnityEngine.UI.Button>().interactable = canContinueMission;
        }

        public void DisplayGameplayMenu()
        {
            this.initAllPopUp();
            this._gameplayMenuGO.SetActive(true);
            // Make shadow clickable
            _popUpShawdowGO.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        public void DisplaySoftLoading()
        {
            this.initAllPopUp();
            this._softLoadingGO.SetActive(true);
        }
    }
}


