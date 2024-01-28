using System;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Construction.FTB
{
    public enum FTBInputType { CHOICE, TYPING }
    public interface IInputTokenController
    {
        void SetUpToken(FillTheBlankToken FTB, FTBInputType type);
    }
    public class InputTokenController : GameplayController, IInputTokenController
    {
        [Header("Input configure")]
        [SerializeField] private GameObject _selectInputGO;
        [SerializeField] private GameObject _typingInputGO;

        private string[] _currContextOptions;

        private FillTheBlankToken _currFTB;
        public void UpdateTypingInput(string given) => _currFTB.SetSelectedString(given);

        public void SetUpToken(FillTheBlankToken FTB, FTBInputType type)
        {
            _selectInputGO.SetActive(false);
            _typingInputGO.SetActive(false);

            _currFTB = FTB;
            switch (type)
            {
                case FTBInputType.CHOICE:
                    _currContextOptions = _currFTB.GetContextOptions();
                    TMP_Dropdown selectInputDropdown = mustGetComponent<TMP_Dropdown>(_selectInputGO);
                    setUpDowndrop(selectInputDropdown, _currContextOptions);
                    _selectInputGO.SetActive(true);
                    break;
                case FTBInputType.TYPING:
                    _typingInputGO.SetActive(true);
                    break;
                default: throw new System.Exception(type.ToString() + " is not implemented");
            }
        }

        #region AUX methods
        private void setUpDowndrop(TMP_Dropdown dropdown, string[] options)
        {
            var placeholderOption = new TMP_Dropdown.OptionData("Select a option");
            //Add placeholder
            dropdown.options.Add(placeholderOption);
            foreach(string option in options) dropdown.options.Add(new TMP_Dropdown.OptionData(option));
            dropdown.onValueChanged.AddListener(delegate
                {
                    _currFTB.SetSelectedString((dropdown.value == 0)? string.Empty : options[dropdown.value-1]);
                }
            );
        }
        #endregion

        #region Unity Basic
        
        #endregion
    }
}


