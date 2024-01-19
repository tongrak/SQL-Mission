using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

namespace Gameplay.UI
{
    public enum ActionButtonType { EXECUTION, PROCEED, STEP_BACK, INACTICE }

    public interface IActionButtonController
    {
        bool Activivity { get;}
        void SetActivity(bool activivity);
        ActionButtonType ActionButtonType { get;}
        void SetActionButtonType(ActionButtonType buttonType);

    }

    public class GeneralActionButtonController : GameplayController, IActionButtonController
    {
        [Header("Action Button Resources")]
        [SerializeField] private Sprite _executionSprite;
        [SerializeField] private Sprite _disableExecutionSprite;
        [SerializeField] private Sprite _proceedSprite;
        [SerializeField] private Sprite _stepBackSprite;
        [Header("Button Image")]
        [SerializeField] private UnityEngine.UI.Image _buttonImage;


        private Sprite getFromActionButtonType(ActionButtonType givenType)
        {
            switch (givenType)
            {
                case ActionButtonType.EXECUTION: return _executionSprite;
                case ActionButtonType.PROCEED: return _proceedSprite;
                case ActionButtonType.STEP_BACK: return _stepBackSprite;
                case ActionButtonType.INACTICE: return _disableExecutionSprite;
            }
            throw new System.Exception("Given type is invalid");
        }

        private bool _isActive = false;
        public bool Activivity
        {
            get => _isActive;
            set
            {
                _isActive = value;
                _buttonImage.sprite = _isActive ? getFromActionButtonType(_currentType) : _disableExecutionSprite;
            }
        }
        private ActionButtonType _currentType;
        public ActionButtonType ActionButtonType
        {
            get => _currentType;
            set
            {
                _currentType = value;
                _buttonImage.sprite = _isActive ? getFromActionButtonType(_currentType) : _disableExecutionSprite;
            }
        }
        public void SetActivity(bool activivity) => _isActive = activivity;
        public void SetActionButtonType(ActionButtonType buttonType) => _currentType = buttonType;


    }
}


