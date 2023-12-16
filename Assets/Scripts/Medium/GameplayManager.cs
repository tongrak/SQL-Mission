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
    public interface IGameplayManager
    {
        /// <summary>
        /// Activate Gameplay session
        /// </summary>
        void StartGameplay();
        void ClickExecution();
        //void ClickSendResult();
        void AdvanceAStep();
    }

    public interface IGameplayUILogic
    {
        void updateActionButtonSprite(TabType currentTab, bool canProceed);
    }

    public class GameplayManager : GameplayController, IGameplayManager
    {
        [Header("BE Configuration")]
        [SerializeField] private string _backEndHolderName;

        [Header("UI GameObjects")]
        [SerializeField] private GameObject _dialogBoxControllerObject;
        [SerializeField] private GameObject _mainConsoleControllerObject;
        [SerializeField] private GameObject _dynamicVisualFeedbackObject;
        [SerializeField] private GameObject _consoleTabsObject;
        [SerializeField] private GameObject _actionButtonObject;

        //===== UI Controller =====
        private IDialogBoxController _dialogBoxController => mustGetComponent<IDialogBoxController>(_dialogBoxControllerObject);
        private IMainConsoleController _mainConsoleController => mustGetComponent<IMainConsoleController>(_mainConsoleControllerObject);
        private IConsoleTabsController _consoleTabsController => mustGetComponent<IConsoleTabsController>(_consoleTabsObject);
        private IActionButtonController _actionButtonController => mustGetComponent<IActionButtonController>(_actionButtonObject);
        //===== Visual Controller =====
        private IDynamicVisualController _dynamicVisualController => mustGetComponent<IDynamicVisualController>(_dynamicVisualFeedbackObject);

        //===== BE interface =====
        private IStepController _currStepCon => mustFindComponentOfName<IStepController>(_backEndHolderName);
        private PuzzleManager _currPM => mustFindComponentOfName<PuzzleManager>(_backEndHolderName);
        private IDialogController _currDC => mustFindComponentOfName<IDialogController>(_backEndHolderName);
        private IImageController _currIC => mustFindComponentOfName<IImageController>(_backEndHolderName);

        //===== Injected gameplaylogic =====
        private IGameplayUILogic _gameplayUI => new BasicUILogic(_actionButtonController);

        //===== Runtime Variables =====
        private int _currStepIndex = 0;
        private IPuzzleController _currPC;
        private bool _gameplayIsStarted = false;
        private bool _canAdvanceAStep = false;

        private void actAccordingToStep(GameStep gStep)
        {
            string[] rawImagePaths = _currIC.GetImages(_currStepIndex);
            string[] imagePaths = rawImagePaths.Select(rawImagePathConversion).ToArray();
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

        public void StartGameplay()
        {
            _gameplayIsStarted = true;
            _actionButtonController.Activivity = true;
            actAccordingToStep(_currStepCon.GetCurrentStep());
        }

        #region Aux methods
        /// <summary>
        /// Convert full path into a resource path.
        /// </summary>
        /// <param name="rawImagePath">string representing full path for a image</param>
        /// <returns>Resource path based on given full path</returns>
        private string rawImagePathConversion(string rawImagePath)
        {
            string[] pathTokens = rawImagePath.Split('\\');
            bool foundResources = false;
            //remove all leading folder included resources
            //replace backslash with normal one
            string leadlessPath = pathTokens.Aggregate((acc, x) =>
            {
                if (foundResources) return acc.Equals(string.Empty) ? x : acc + '/' + x;
                else if (x.Equals("Resources"))
                {
                    foundResources = true;
                    return string.Empty;
                }
                else return string.Empty;
            });
            //remove file type and return
            return leadlessPath.Split('.')[0];
        }
        #endregion

        //TODO: Add gameplay action logic
        #region Player actions
        public void ClickExecution()
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

            switch (_consoleTabsController.CurrentTab) 
            {
                case TabType.CONSTRUCT:
                    var result = _currPC.GetExecuteResult(_mainConsoleController.getCurrentQueryString());
                    if (result.TableResult != null)
                    {
                        string[] rawImagePaths = result.TableResult[0];
                        //remove image label
                        rawImagePaths = rawImagePaths.Skip(1).ToArray();
                        string[] imagePaths = rawImagePaths.Select(x => x.Split('.')[0]).ToArray();
                        if (imagePaths.Length > 0) _dynamicVisualController.ShowUpGivenItem(imagePaths);
                    }
                    _canAdvanceAStep = _currPC.GetPuzzleResult();
                    _consoleTabsController.CurrentTab =  TabType.RESULT;
                    _gameplayUI.updateActionButtonSprite(TabType.RESULT, _canAdvanceAStep);
                    _mainConsoleController.setResultDisplay(_currPC.GetPuzzleResult(), result);
                    break;
                case TabType.RESULT:
                    if (_canAdvanceAStep) AdvanceAStep();
                    _consoleTabsController.CurrentTab =  TabType.CONSTRUCT;
                    _gameplayUI.updateActionButtonSprite(TabType.CONSTRUCT, _canAdvanceAStep);
                    _mainConsoleController.setDisplayTab(TabType.CONSTRUCT);
                    break;
            }

            
            //If player can, let them advance.
        }
        public void AdvanceAStep()
        {
            if (!_gameplayIsStarted)
            {
                Debug.LogWarning("gameplay is inactive");
                return;
            }
            _dynamicVisualController.DiscontinueItemObjects();
            _consoleTabsController.CurrentTab = TabType.CONSTRUCT;

            _currStepCon.ChangeStep();
            _currStepIndex++;

            _consoleTabsController.CurrentTab = TabType.CONSTRUCT;
            _gameplayUI.updateActionButtonSprite(TabType.CONSTRUCT, _canAdvanceAStep);


            actAccordingToStep(_currStepCon.GetCurrentStep());
        }
        #endregion

        #region Unity Basic
        private void Start()
        {
            _mainConsoleController.setDisplayTab(TabType.CONSTRUCT);
            _actionButtonController.Activivity = false;
        }
        #endregion
    }

}