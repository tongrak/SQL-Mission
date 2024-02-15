using Gameplay;
using Gameplay.UI.PopUp;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.PopUp
{
    public interface IPlacementPopUpManager : IPopUpsManager
    {
        void DisplayGiveUpMenu();
        void HideGiveUpMenu();
    }
    public class PlacementPopUpManager : GameplayController, IPlacementPopUpManager
    {
        [Header("Gameobjects")]
        [SerializeField] private GameObject _giveUpMenuGO;
        [SerializeField] protected GameObject _softLoadingGO;
        [SerializeField] private GameObject _popUpShadowGO;

        public void initPopUp()
        {
            this.gameObject.SetActive(true);
            _giveUpMenuGO.SetActive(false);
            _softLoadingGO.SetActive(false);

            _popUpShadowGO.SetActive(true);
        }

        public void DisplayGiveUpMenu()
        {
            this.initPopUp();
            _giveUpMenuGO.SetActive(true);
            _popUpShadowGO.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }

        public void DisplaySoftLoading()
        {
            this.initPopUp();
            this._softLoadingGO.SetActive(true);
        }

        public void HideGiveUpMenu()
        {
            _giveUpMenuGO.SetActive(false);
            _popUpShadowGO.SetActive(false);
        }

        public void DisplayCompletionMenu(bool canContinueMission){}
    }
}