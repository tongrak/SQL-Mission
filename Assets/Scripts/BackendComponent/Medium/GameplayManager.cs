using Assets.Scripts.DataPersistence.DialogController;
using Assets.Scripts.DataPersistence.ImageController;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.PuzzleController;
using Assets.Scripts.DataPersistence.PuzzleManager;
using Assets.Scripts.DataPersistence.StepController;
using Gameplay.UI;
using Gameplay.UI.PopUp;
using Gameplay.UI.VisualFeedback;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Gameplay
{
    public interface IGameplayManager
    {
        /// <summary>
        /// Activate Gameplay session without saving the progress. required triggering = EndGameplay().
        /// </summary>
        void StartFreeGame();
        /// <summary>
        /// Activate Normal mission Gameplay session. required triggering = EndGameplay().
        /// </summary>
        void StartNormalGameplay(FileSystemWatcher MissionFileWatcher);
        /// <summary>
        /// Activate Final mission Gameplay session. required triggering = EndGameplay().
        /// </summary>
        void StartFinalGameplay(FileSystemWatcher MissionFileWatcher, FileSystemWatcher ChapterFileWatcher);
        /// <summary>
        /// Activate Placement test Gameplay session.
        /// </summary>
        void StartPlacement(FileSystemWatcher ChapterFileWatcher);
        /// <summary>
        /// Confirm the ending of gameplay
        /// </summary>
        /// <param name="canGoToNextMission">Activity of Next Misison</param>
        void EndGameplay(bool canGoToNextMission);


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
        [SerializeField] private GameObject _staticVisualFeedbackObject;
        [SerializeField] private GameObject _consoleTabsObject;
        [SerializeField] private GameObject _actionButtonObject;
        [SerializeField] private GameObject _schemaDisplayObject;
        [SerializeField] private GameObject _stepTraverseObject;

        [Header("Misc GameObject")]
        [SerializeField] private GameObject _popUpManagerObject;

        //===== UI Controller =====
        private IDialogBoxController _dialogBoxController => mustGetComponent<IDialogBoxController>(_dialogBoxControllerObject);
        private IMainConsoleController _mainConsoleController => mustGetComponent<IMainConsoleController>(_mainConsoleControllerObject);
        private IConsoleTabsController _consoleTabsController => mustGetComponent<IConsoleTabsController>(_consoleTabsObject);
        private IActionButtonController _actionButtonController => mustGetComponent<IActionButtonController>(_actionButtonObject);
        private ISchemaDisplayController _schemaDisplayController => mustGetComponent<ISchemaDisplayController>(_schemaDisplayObject);
        private IGameplayStepDisplayController _stepTraversesController => mustGetComponent<IGameplayStepDisplayController>(_stepTraverseObject);
        //===== Visual Controller =====
        private IDynamicVisualController _dynamicVisualController => mustGetComponent<IDynamicVisualController>(_dynamicVisualFeedbackObject);
        private IStaticVisualController _staticVisualController => mustGetComponent<IStaticVisualController>(_staticVisualFeedbackObject);
        //===== Misc Controller =====
        private IPopUpsManager _popUpsManager => mustGetComponent<IPopUpsManager>(_popUpManagerObject);

        //===== BE interface =====
        private IStepController _currStepCon => mustGetComponent<IStepController>(_backEndObject);
        private PuzzleManager _currPM => mustGetComponent<PuzzleManager>(_backEndObject);
        private IDialogController _currDC => mustGetComponent<IDialogController>(_backEndObject);
        private IImageController _currIC => mustGetComponent<IImageController>(_backEndObject);

        //===== Injected gameplaylogic =====
        private IGameplayUILogic _gameplayUI => new BasicUILogic(_consoleTabsController, _actionButtonController);

        //===== Runtime Variables =====
        private int _totalStepCount = 0;
        private int _currStepIndex = 0;
        private IPuzzleController _currPC;
        private bool _gameplayIsStarted = false;
        private bool _canAdvanceAStep = false;
        private TabType _currentTab = TabType.CONSTRUCT;
        private bool _isPlacementTest = false;
        private bool? _canGoNextMission = null;
        // ===== save Wacther Variable ===
        private System.Tuple<string, FileSystemWatcher>[] _namedSaveFileWatchers = null;
        private System.Collections.Generic.List<string> _savedFileWatcherNames = null;


        private void actAccordingToStep(GameStep gStep)
        {
            //Prep given image path
            string[] rawImagePaths = null;
            string[] imagePaths = null;
            //Check if given raw images paths is accessable
            try
            { 
                rawImagePaths = _currIC.GetImages(_currStepIndex);
                if (rawImagePaths != null && rawImagePaths[0] != null) imagePaths = rawImagePaths.Select(rawImagePathConversion).ToArray();
            } catch (System.Exception e) { Debug.LogWarning(e.ToString()); }

            switch (gStep.CurrStep)
            {
                case Step.EndStep:
                    Debug.Log("Reaching end step");
                    _gameplayIsStarted = false;
                    _canAdvanceAStep = false;
                    _gameplayUI.SetDisplayedActionButton(ActionButtonType.INACTICE);
                    
                    _currStepIndex--;
                    //Switch to boards scnene if appropriate
                    StartCoroutine(startLoadingThenConpletion(_maxLoadingSecond));
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

                    SelectConstructionTab();
                    if (imagePaths != null) handleOnVisualType(_currPC.VisualType, imagePaths);

                    _canAdvanceAStep = false;
                    break;
                case Step.Dialog:
                    Debug.Log("Reaching dialog step");
                    string dialog = _currDC.GetDialog(gStep.DialogIndex);
                    _dialogBoxController.displayedText = dialog;
                    _canAdvanceAStep = true;
                    _gameplayUI.SetDisplayedActionButton(ActionButtonType.PROCEED);

                    if (imagePaths != null) handleOnVisualType(VisualType.Static, imagePaths);

                    break;
            }
            
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
            System.Func<string, string[]> getOptions = pC.GetBlankOptions;
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
        private void handleOnVisualType(VisualType type, string[] imagePaths)
        {
            //Hide Visual feedback
            _dynamicVisualFeedbackObject.SetActive(false);
            _staticVisualFeedbackObject.SetActive(false);

            switch (type)
            {
                case VisualType.Dynamic: 
                    _dynamicVisualFeedbackObject.SetActive(true);
                    _dynamicVisualController.InitItemObjects(imagePaths); 
                    break;
                case VisualType.Static:
                    _staticVisualFeedbackObject.SetActive(true);
                    _staticVisualController.InitItemObjects(imagePaths); 
                    break;
            }
        }
        private bool allIsSaved => (_namedSaveFileWatchers == null) ? true : _namedSaveFileWatchers.Count() == _savedFileWatcherNames?.Count();
        private void unsubAllSaveFileWatcher()
        {
            if (_namedSaveFileWatchers != null) 
                foreach (var namedWatcher in _namedSaveFileWatchers) namedWatcher.Item2.Changed -= (s, e) => _savedFileWatcherNames.Add(namedWatcher.Item1);
        }
        private IEnumerator startLoadingThenConpletion(int maxLoadingSeconds)
        {
            // Wait untill game result is saved
            var startedTime = System.DateTime.Now;
            var currentTime = System.DateTime.Now;
            // Show loading facade.
            _popUpManagerObject.SetActive(true);
            _popUpsManager.DisplaySoftLoading();
            // While all file isnot saved and canGoNextMission flag have been raise;
            while (!(allIsSaved && _canGoNextMission != null) &&
                    (currentTime - startedTime).Seconds < maxLoadingSeconds)
            {
                currentTime = System.DateTime.Now;
                yield return null;
            }

            unsubAllSaveFileWatcher();

            if (_isPlacementTest)_scenesManager.LoadSelectChapterScene();
            else _popUpsManager.DisplayCompletionMenu(_canGoNextMission ?? false);
        }
        private void updateGameplayStepController()
        {
            _stepTraversesController.UpdateStepTextDisplay(_currStepIndex + 1, _totalStepCount - 1);
            _stepTraversesController.UpdateStepTraverseState(_canAdvanceAStep, _currStepIndex > 0);
        }        
        #endregion

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
                    _canAdvanceAStep = _currPC.GetPuzzleResult().IsCorrect;
                    //Update Next button if player answer correctly
                    _stepTraversesController.UpdateStepTraverseState(_canAdvanceAStep, _currStepIndex > 0);

                    _mainConsoleController.setResultDisplay(result, _currPC.GetPuzzleResult(), _currPC.AnswerTableResult);
                    SelectResultTab();
                    break;
                case TabType.RESULT:
                    SelectConstructionTab();
                    if (_canAdvanceAStep) AdvanceAStep();
                    break;
            }
        }
        private void handleChangeStepAction(System.Action stepChangingAction)
        {
            _dynamicVisualController.DiscontinueVisualItemObjects();
            _staticVisualController.DiscontinueVisualItemObjects();

            stepChangingAction();

            SelectConstructionTab();
            actAccordingToStep(_currStepCon.GetCurrentStep());
            //Update step traverse button state
            updateGameplayStepController();
        }
        public void AdvanceAStep() => handleChangeStepAction(() =>
        {
            _currStepCon.ChangeStep();
            _currStepIndex++;
        });
        public void RetreatAStep() => handleChangeStepAction(() =>
        {
            _currStepCon.GoBackPreviousStep();
            _currStepIndex--;
        });
        private void setToGivenTab(TabType tab)
        {
            _gameplayUI.UpdateUIDisplay(tab, _canAdvanceAStep);
            _mainConsoleController.setDisplayTab(tab);
            _currentTab = tab;
        }
        public void SelectConstructionTab() => setToGivenTab(TabType.CONSTRUCT);
        public void SelectResultTab() => setToGivenTab(TabType.RESULT);
        #endregion

        #region Interface functions
        private void startGameplayScene(System.Tuple<string, FileSystemWatcher>[] namedSaveFileWatchers)
        {
            //init gameplay
            _namedSaveFileWatchers = null;
            _savedFileWatcherNames = null;
            _gameplayIsStarted = true;
            _actionButtonController.Activivity = true;
            //Collection and subscribe every given saveFileWatcher;
            if (namedSaveFileWatchers != null)
                foreach (var namedWatcher in namedSaveFileWatchers) namedWatcher.Item2.Changed += (s, e) => _savedFileWatcherNames.Add(namedWatcher.Item1);
            _namedSaveFileWatchers = namedSaveFileWatchers;
            //Set up display
            _currStepIndex = 0;
            _totalStepCount = _currStepCon.AllGameStep.Length;
            updateGameplayStepController();
            //Begin gameplay
            actAccordingToStep(_currStepCon.GetCurrentStep());
        }
        public void StartFreeGame() => startGameplayScene(null);
        public void StartNormalGameplay(FileSystemWatcher MissionFileWatcher)
        {
            var namedSaveFileWatcher = new System.Tuple<string, FileSystemWatcher>("MissionSave", MissionFileWatcher);
            startGameplayScene(new System.Tuple<string, FileSystemWatcher>[] { namedSaveFileWatcher });
        }
        public void StartFinalGameplay(FileSystemWatcher MissionFileWatcher, FileSystemWatcher ChapterFileWatcher)
        {
            var missionFileWatcher = new System.Tuple<string, FileSystemWatcher>("MissionSave", MissionFileWatcher);
            var chapterFileWatcher = new System.Tuple<string, FileSystemWatcher>("ChapterSave", ChapterFileWatcher);
            startGameplayScene(new System.Tuple<string, FileSystemWatcher>[] { missionFileWatcher, chapterFileWatcher });
        }
        public void StartPlacement(FileSystemWatcher ChapterFileWatcher)
        {
            var namedSaveFileWatcher = new System.Tuple<string, FileSystemWatcher>("ChapterSave", ChapterFileWatcher);
            startGameplayScene(new System.Tuple<string, FileSystemWatcher>[] { namedSaveFileWatcher });
            this._isPlacementTest = true;
            this._canGoNextMission = false;
        }
        public void EndGameplay(bool canGoToNextMission) => this._canGoNextMission = canGoToNextMission;
        #endregion

        #region Unity Basic
        private void Start()
        {
            //Hide loading facade;
            _popUpManagerObject.SetActive(false);
        }
        private void OnDisable() => unsubAllSaveFileWatcher();
        private void OnDestroy() => unsubAllSaveFileWatcher();
        #endregion
    }
}