using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.DialogController;
using Assets.Scripts.BackendComponent.ImageController;
using Assets.Scripts.BackendComponent.StepComponent;
using Gameplay.UI;
using Gameplay.UI.VisualFeedback;
using System.Linq;
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
        [Header("BE Configuration")]
        [SerializeField] private string _backEndHolderName;

        [Header("UI GameObjects")]
        [SerializeField] private GameObject _dialogBoxControllerObject;
        [SerializeField] private GameObject _mainConsoleControllerObject;
        [SerializeField] private GameObject _dynamicVisualFeedbackObject;

        private IDialogBoxController _dialogBoxController => mustGetComponent<IDialogBoxController>(_dialogBoxControllerObject);
        private IMainConsoleController _mainConsoleController => mustGetComponent<IMainConsoleController>(_mainConsoleControllerObject);
        // Visual Controller
        private IDynamicVisualController _dynamicVisualController => mustGetComponent<IDynamicVisualController>(_dynamicVisualFeedbackObject);

        private IStepController _currStepCon => mustFindComponentOfName<IStepController>(_backEndHolderName);
        private PuzzleManager _currPM => mustFindComponentOfName<PuzzleManager>(_backEndHolderName);
        private IDialogController _currDC => mustFindComponentOfName<IDialogController>(_backEndHolderName);
        private IImageController _currIC => mustFindComponentOfName<IImageController>(_backEndHolderName);

        private int _currStepIndex = 0;
        private IPuzzleController _currPC;
        private ExecuteResult _currExeResult;

        private bool _gameplayIsStarted = false;
        private bool _canAdvanceAStep = false;

        private void actAccordingToStep(GameStep gStep)
        {
            string[] rawImagePaths = _currIC.GetImages(_currStepIndex);
            string[] imagePaths = rawImagePaths.Select(x => x.Split('.')[0]).ToArray();

            switch (gStep.CurrStep)
            {
                case Step.EndStep:
                    Debug.Log("Reaching end step");
                    _gameplayIsStarted = false;
                    _canAdvanceAStep = false;
                    break;
                case Step.Puzzle:
                    Debug.Log("Reaching puzzle step");
                    _currPC = _currPM.GetPC(gStep.PCIndex);
                    _dialogBoxController.displayedText = _currPC.Brief;
                    //TODO: Check for visual type.
                    _dynamicVisualController.InitItemObjects(imagePaths);

                    _canAdvanceAStep = false;
                    break;
                case Step.Dialog:
                    Debug.Log("Reaching dialog step");
                    string dialog = _currDC.GetDialog(gStep.DialogIndex);
                    _dialogBoxController.displayedText = dialog;
                    _canAdvanceAStep = true;
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

            if (!_gameplayIsStarted)
            {
                Debug.LogWarning("gameplay is inactive");
                return;
            }

            _dynamicVisualController.ShowDownAll();
            // In case of universal button for progression
            if (!_canAdvanceAStep)
            {
                var result = _currPC.GetExecuteResult(_mainConsoleController.getCurrentQueryString());
                if (result.TableResult != null) 
                {
                    string[] rawImagePaths = result.TableResult[0];
                    string[] imagePaths = rawImagePaths.Select(x => x.Split('.')[0]).ToArray();
                    //string[] imagePaths = rawImagePaths.Select(x => getResourcesPathFromFull(x)).ToArray();

                    if (imagePaths.Length > 0)
                        _dynamicVisualController.ShowUpGivenItem(imagePaths);
                }
                _mainConsoleController.setResultDisplay(_currPC.GetPuzzleResult(), result);
                _canAdvanceAStep = _currPC.GetPuzzleResult();
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
            _dynamicVisualController.DiscontinueItemObjects();

            _currStepCon.ChangeStep();
            _currStepIndex++;
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

        private void Start()
        {
            _mainConsoleController.setDisplayTab(TabType.CONSTRUCT);
        }
    }

}