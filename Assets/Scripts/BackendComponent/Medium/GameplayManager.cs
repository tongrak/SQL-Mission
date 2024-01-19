using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.DialogController;
using Assets.Scripts.BackendComponent.ImageController;
using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.BackendComponent.PuzzleController;
using Assets.Scripts.BackendComponent.PuzzleManager;
using Assets.Scripts.BackendComponent.StepController;
using Gameplay.UI;
using Gameplay.UI.VisualFeedback;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public interface IGameplayManager
    {
        /// <summary>
        /// Activate Gameplay session
        /// </summary>
        void StartGameplay(FileSystemWatcher saveFileWatcher);
        void ClickExecution();
        void SelectConstructionTab();
        void SelectResultTab();
        void AdvanceAStep();
    }
    public interface IGameplayUILogic
    {
        void UpdateUIDisplay(TabType currentTab, bool canProceed);
        void SetDisplayedActionButton(ActionButtonType buttonType);
    }

    public class GameplayManager : GameplayController, IGameplayManager
    {
        [Header("Utility Configuration")]
        [SerializeField] private ScenesManager _scenesManager;
        [SerializeField] private int _maxLoadingSecond;

        [Header("BE GameObjects")]
        [SerializeField] private GameObject _backEndObject;

        [Header("UI GameObjects")]
        [SerializeField] private GameObject _dialogBoxControllerObject;
        [SerializeField] private GameObject _mainConsoleControllerObject;
        [SerializeField] private GameObject _dynamicVisualFeedbackObject;
        [SerializeField] private GameObject _consoleTabsObject;
        [SerializeField] private GameObject _actionButtonObject;
        [SerializeField] private GameObject _schemaDisplayObject;
        [SerializeField] private GameObject _loadingFacadeObject;

        //===== UI Controller =====
        private IDialogBoxController _dialogBoxController => mustGetComponent<IDialogBoxController>(_dialogBoxControllerObject);
        private IMainConsoleController _mainConsoleController => mustGetComponent<IMainConsoleController>(_mainConsoleControllerObject);
        private IConsoleTabsController _consoleTabsController => mustGetComponent<IConsoleTabsController>(_consoleTabsObject);
        private IActionButtonController _actionButtonController => mustGetComponent<IActionButtonController>(_actionButtonObject);
        private ISchemaDisplayController _schemaDisplayController => mustGetComponent<ISchemaDisplayController>(_schemaDisplayObject);
        //===== Visual Controller =====
        private IDynamicVisualController _dynamicVisualController => mustGetComponent<IDynamicVisualController>(_dynamicVisualFeedbackObject);

        //===== BE interface =====
        private IStepController _currStepCon => mustGetComponent<IStepController>(_backEndObject);
        private PuzzleManager _currPM => mustGetComponent<PuzzleManager>(_backEndObject);
        private IDialogController _currDC => mustGetComponent<IDialogController>(_backEndObject);
        private IImageController _currIC => mustGetComponent<IImageController>(_backEndObject);

        //===== Injected gameplaylogic =====
        private IGameplayUILogic _gameplayUI => new BasicUILogic(_consoleTabsController, _actionButtonController);

        //===== Runtime Variables =====
        private int _currStepIndex = 0;
        private IPuzzleController _currPC;
        private FileSystemWatcher _saveFileWatcher = null;
        private bool _gameplayIsStarted = false;
        private bool _canAdvanceAStep = false;
        private bool _resultSaved = false;
        private TabType _currentTab = TabType.CONSTRUCT;

        private void actAccordingToStep(GameStep gStep)
        {
            string[] rawImagePaths = null;
            string[] imagePaths = null;
            try { rawImagePaths = _currIC.GetImages(_currStepIndex); } catch (System.Exception e) { Debug.LogWarning(e.ToString()); }
            if (rawImagePaths != null) imagePaths = rawImagePaths.Select(rawImagePathConversion).ToArray();
            switch (gStep.CurrStep)
            {
                case Step.EndStep:
                    Debug.Log("Reaching end step");
                    _gameplayIsStarted = false;
                    _canAdvanceAStep = false;
                    _gameplayUI.SetDisplayedActionButton(ActionButtonType.INACTICE);
                    //Switch to boards scnene if appropriate
                    StartCoroutine(switchToBoardsSceneWhenAppro(_maxLoadingSecond));
                    break;
                case Step.Puzzle:
                    Debug.Log("Reaching puzzle step");
                    _currPC = _currPM.GetPC(gStep.PCIndex);
                    //Set dialog box with puzzle brief.
                    _dialogBoxController.displayedText = _currPC.Brief;
                    //Set console accordingly
                    handleOnConsoleType(_currPC, _currPC.PuzzleType, _currPC.PreSQL);
                    //Set schema display
                    SchemaDTO[] schemaDTOs = _currPC.Schemas.Select(x => { return new SchemaDTO(x.TableName, x.Attributes); }).ToArray();
                    _schemaDisplayController.SetUpDisplay(schemaDTOs);

                    if (imagePaths != null)
                    {
                        if (_currPC.VisualType == VisualType.A)
                        {
                            _dynamicVisualController.InitItemObjects(imagePaths);
                        }
                        else
                        {
                            Debug.LogWarning("Puzzle with VisualType B detected");
                        }
                    }
                    _canAdvanceAStep = false;
                    break;
                case Step.Dialog:
                    Debug.Log("Reaching dialog step");
                    string dialog = _currDC.GetDialog(gStep.DialogIndex);
                    _dialogBoxController.displayedText = dialog;
                    _canAdvanceAStep = true;
                    _gameplayUI.SetDisplayedActionButton(ActionButtonType.PROCEED);
                    break;
            }
        }

        public void StartGameplay(FileSystemWatcher saveFileWatcher)
        {
            //TODO: Utilize given fileWatcher for the save "scene" thingie...
            _gameplayIsStarted = true;
            _actionButtonController.SetActivity(true);
            actAccordingToStep(_currStepCon.GetCurrentStep());
            _saveFileWatcher = saveFileWatcher;
            _saveFileWatcher.Changed += onSaveComplete;
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
        private void handleOnConsoleType(IPuzzleController pC, PuzzleType type, string tokens)
        {
            //TODO: Wait for BE access function
            Func<string, string[]> getOptions = null;
            switch (type)
            {
                case PuzzleType.ExecuteOnly:
                    _mainConsoleController.setConstructionDisplay(getOptions, UI.Construction.ConstructionType.FILL_THE_BLANK, tokens);
                    break;
                case PuzzleType.FillBlank: 
                    _mainConsoleController.setConstructionDisplay(getOptions, UI.Construction.ConstructionType.FILL_THE_BLANK, tokens);
                    break;
                case PuzzleType.OnYourOwn:
                    _mainConsoleController.setConstructionDisplay(getOptions, UI.Construction.ConstructionType.TYPING, tokens);
                    break;
            }
        }
        private void onSaveComplete(object source, FileSystemEventArgs e) => _resultSaved = true;
        private IEnumerator switchToBoardsSceneWhenAppro(int maxLoadingSeconds)
        {
            // Wait untill game result is saved
            var startedTime = DateTime.Now;
            var currentTime = DateTime.Now;
            // Show loading facade.
            _loadingFacadeObject.SetActive(true);
            while (!_resultSaved && (currentTime - startedTime).Seconds < maxLoadingSeconds)
            {
                currentTime = DateTime.Now;
                var currDiff = (currentTime - startedTime).Seconds;
                yield return null;
            }
            //TODO: check loading status;
            _loadingFacadeObject.SetActive(false);
            //Then switch back to Boards Scene
            _scenesManager.LoadSelectMissionScene();
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

            if (_currStepCon.GetCurrentStep().CurrStep != Step.Puzzle)
            {
                AdvanceAStep();
                return;
            }

            switch (_currentTab)
            {
                case TabType.CONSTRUCT:
                    ExecuteResult result = _currPC.GetExecuteResult(_mainConsoleController.getCurrentQueryString());
                    if (result.TableResult != null)
                    {
                        string[] rawImagePaths = result.TableResult[0];
                        //remove image label
                        rawImagePaths = rawImagePaths.Skip(1).ToArray();
                        string[] imagePaths = rawImagePaths.Select(x => x.Split('.')[0]).ToArray();
                        if (imagePaths.Length > 0) _dynamicVisualController.ShowUpGivenItem(imagePaths);
                    }
                    _canAdvanceAStep = _currPC.GetPuzzleResult();
                    ExecuteResultDTO executeResultDTO = new ExecuteResultDTO((result.IsError, result.ErrorMessage), result.TableResult);
                    _mainConsoleController.setResultDisplay(_currPC.GetPuzzleResult(), executeResultDTO);
                    SelectResultTab();
                    break;
                case TabType.RESULT:
                    SelectConstructionTab();
                    if (_canAdvanceAStep) AdvanceAStep();
                    break;
            }
        }
        public void AdvanceAStep()
        {
            _dynamicVisualController.DiscontinueItemObjects();

            _currStepCon.ChangeStep();
            _currStepIndex++;

            SelectConstructionTab();
            actAccordingToStep(_currStepCon.GetCurrentStep());
        }
        private void setToGivenTab(TabType tab)
        {
            _gameplayUI.UpdateUIDisplay(tab, _canAdvanceAStep);
            _mainConsoleController.setDisplayTab(tab);
            _currentTab = tab;
        }
        public void SelectConstructionTab() => setToGivenTab(TabType.CONSTRUCT);
        public void SelectResultTab() => setToGivenTab(TabType.RESULT);
        #endregion

        #region Unity Basic
        private void Start()
        {
            //Hide loading facade;
            _loadingFacadeObject.SetActive(false);
            SelectConstructionTab();
            _actionButtonController.SetActivity(false);
        }
        private void OnDisable()
        {
            if (_saveFileWatcher != null) _saveFileWatcher.Changed -= onSaveComplete;
        }
        private void OnDestroy()
        {
            if (_saveFileWatcher != null) _saveFileWatcher.Changed -= onSaveComplete;
        }
        #endregion
    }
}