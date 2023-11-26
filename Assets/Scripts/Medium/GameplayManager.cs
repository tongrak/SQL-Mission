using Assets.Scripts.PuzzleComponent;
using Assets.Scripts.PuzzleComponent.StepComponent;
using Gameplay.UI;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public interface IGamePlayerManager
    {
        /// <summary>
        /// Activate Gameplay session
        /// </summary>
        void startGameplay();
        void clickExecution();
        void clickSendResult();
        void advanceAStep();
    }

    public class GameplayManager : GameplayController, IGamePlayerManager
    {
        [Header("UI GameObjects")]
        [SerializeField] private GameObject _dialogBoxControllerObject;
        [SerializeField] private GameObject _mainConsoleControllerObject;

        private IDialogBoxController _dialogBoxController => mustGetComponent<IDialogBoxController>(_dialogBoxControllerObject);
        private IMainConsoleController _mainConsoleController => mustGetComponent<IMainConsoleController>(_mainConsoleControllerObject);

        private IStepController _currStepCon => mustGetComponent<IStepController>();
        private PuzzleManager _currPM => mustGetComponent<PuzzleManager>();
        private DialogController _currDC => mustGetComponent<DialogController>();

        private IPuzzleController _currPC;
        private ExecuteResult _currExeResult;

        private bool _gameplayIsStarted = false;
        private bool _canAdvanceAStep = false;

        private void actAccordingToStep(GameStep gStep)
        {
            switch (gStep.CurrStep)
            {
                case Step.EndStep:
                    Debug.Log("Reaching end step");
                    // wrap thing up
                    _gameplayIsStarted = false;
                    break;
                case Step.Puzzle:
                    Debug.Log("Reaching puzzle step");
                    _currPC = _currPM.GetPC(gStep.PCIndex);
                    _canAdvanceAStep = false;
                    // begin the puzzle solving sequence
                    _dialogBoxController.displayedText = _currPC.Brief;
                    break;
                case Step.Dialog:
                    Debug.Log("Reaching dialog step");
                    string dialog = _currDC.GetDialog(gStep.DialogIndex);
                    _dialogBoxController.displayedText = dialog;
                    _canAdvanceAStep = true;
                    // When
                    break;
            }
        }

        public void startGameplay()
        {
            _gameplayIsStarted = true;

            actAccordingToStep(_currStepCon.GetCurrentStep());
        }

        #region Player actions
        public void clickExecution()
        {
            Debug.Log("Execution required receive");
            Debug.Log("Query: " + _mainConsoleController.getCurrentQueryString());

            if( !_gameplayIsStarted )
            {
                Debug.LogWarning("gameplay is inactive");
                return;
            }

            // In case of universal button for progression
            if (!_canAdvanceAStep)
            {
                var result = _currPC.GetExecuteResult(_mainConsoleController.getCurrentQueryString());
                _mainConsoleController.setResultDisplay(_currPC.IsPass, result);
                _canAdvanceAStep = _currPC.IsPass;

                return;
            }
            //If player can, let them advance.
            advanceAStep();
        }
        public void advanceAStep()
        {
            if (!_gameplayIsStarted)
            {
                Debug.LogWarning("gameplay is inactive");
                return;
            }

            _currStepCon.ChangeStep();
            actAccordingToStep(_currStepCon.GetCurrentStep());
        }
        public void clickSendResult()
        {
            if (!_gameplayIsStarted)
            {
                Debug.LogWarning("gameplay is inactive");
                return;
            }

            Debug.Log("result passing request received");
            advanceAStep();
        }
        #endregion
    }
}