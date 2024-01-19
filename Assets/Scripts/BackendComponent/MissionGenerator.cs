using Assets.Scripts.BackendComponent.BlankBlockComponent;
using Assets.Scripts.BackendComponent.DialogController;
using Assets.Scripts.BackendComponent.SQLComponent;
using System.Linq;
using UnityEngine;
using Assets.Scripts.BackendComponent.ImageController;
using System.IO;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Helper;
using Assets.Scripts.BackendComponent.StepController;
using Assets.Scripts.BackendComponent.PuzzleManager;
using Assets.Scripts.BackendComponent.Model;
using System;
using Gameplay;

namespace Assets.Scripts.BackendComponent
{
    public class MissionGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogConGameObject;
        [SerializeField] private GameObject _stepControllerGameObject;
        [SerializeField] private GameObject _puzzleManagerGameObject;
        [SerializeField] private GameObject _imageControllerGameObject;
        [SerializeField] private GameObject _missionControllerGameObject;
        [SerializeField] private GameObject _gameplayManagerGameObjefct;
        [SerializeField] private MissionData _missionSceneData;
        [SerializeField] private TextAsset _configFile;
        [SerializeField] private bool _isMock;

        private MissionConfig _missionConfig;
        private ISQLService _sqlService = new SQLService();
        private IFixedTemplateService _fixedTemplateService = new FixedTemplateService();
        private IUpToConfigTemplateService _upToConfigTemplateService;

        private void StartGenerating()
        {
            _upToConfigTemplateService = new UpToConfigTemplateService(_sqlService);

            if (_isMock)
            {
                _MockLoadConfigFile();
            }
            else
            {
                LoadConfigFile();
            }
            LoadDialogController();
            LoadStepController();
            LoadPuzzleManager();
            LoadImageController();
            _InitiateMissionController();
        }

        #region Method for StartGenerating
        private void _MockLoadConfigFile()
        {
            _missionConfig = JsonUtility.FromJson<MissionConfig>(_configFile.text);
        }

        private void LoadConfigFile()
        {
            string folderPathAfterResources = _missionSceneData.MissionConfigFolderFullPath.Split(new string[] { "Resources/" }, StringSplitOptions.None)[1];
            TextAsset missionConfigFile = Resources.Load<TextAsset>(folderPathAfterResources + "/" + _missionSceneData.MissionFileName);
            _missionConfig = JsonUtility.FromJson<MissionConfig>(missionConfigFile.text);
        }

        private void LoadDialogController()
        {
            IDialogController dialogController = _dialogConGameObject.GetComponent<IDialogController>();
            dialogController.SetAllDialog(_missionConfig.MissionDetail.Where(x => x.Step == Step.Dialog).Select(x => x.Dialog).ToArray());
        }

        private void LoadStepController()
        {
            IStepController stepController = _stepControllerGameObject.GetComponent<IStepController>();
            Step[] allConfigStep = _missionConfig.MissionDetail.Select(x => x.Step).ToArray();

            int dialogIndex = 0;
            int puzzleIndex = 0;
            GameStep[] allGameStep = new GameStep[allConfigStep.Length + 1]; // Add slot for end step.
            for (int i = 0; i < allConfigStep.Length; i++)
            {
                Step step = allConfigStep[i];
                switch (step)
                {
                    case Step.Dialog:
                        allGameStep[i] = new GameStep(Step.Dialog, i, dialogIndex, -1);
                        dialogIndex++;
                        break;
                    case Step.Puzzle:
                        allGameStep[i] = new GameStep(Step.Puzzle, i, -1, puzzleIndex);
                        puzzleIndex++;
                        break;
                    default:
                        break;
                }
            }

            int lastStepIndex = allGameStep.Length - 1;
            allGameStep[lastStepIndex] = new GameStep(Step.EndStep, lastStepIndex, -1, -1);

            stepController.SetAllGameStep(allGameStep);
        }

        private void LoadPuzzleManager()
        {
            IPuzzleManager puzzleManager = _puzzleManagerGameObject.GetComponent<IPuzzleManager>();
            StepDetail[] allPuzzleStepDetail = _missionConfig.MissionDetail.Where(x => x.Step == Step.Puzzle).ToArray();
            PuzzleController.PuzzleController[] allPuzzleController = new PuzzleController.PuzzleController[allPuzzleStepDetail.Length];

            for(int i = 0; i < allPuzzleStepDetail.Length; i++)
            {
                StepDetail puzzleStepDetail = allPuzzleStepDetail[i];
                // 1) Create database path
                string dbFolder = $"/Resources/{EnvironmentData.Instance.DatabaseRootFolder}/";
                string dbConn = "URI=file:" + Application.dataPath + dbFolder + puzzleStepDetail.PuzzleDetail.DB;
                // 2) Get schema from SQLService
                Schema[] schemas = _sqlService.GetSchemas(dbConn, puzzleStepDetail.PuzzleDetail.Tables);
                // 3) Create PuzzleController
                PuzzleController.PuzzleController puzzleController = new PuzzleController.PuzzleController(dbConn, puzzleStepDetail.PuzzleDetail.AnswerSQL, puzzleStepDetail.Dialog, schemas, _sqlService, puzzleStepDetail.PuzzleDetail.VisualType, _fixedTemplateService, _upToConfigTemplateService, puzzleStepDetail.PuzzleDetail.BlankOptions, puzzleStepDetail.PuzzleDetail.PreSQL, puzzleStepDetail.PuzzleDetail.PuzzleType);
                // 4) Insert PuzzleController to array.
                allPuzzleController[i] = puzzleController;
            }
            // 5) Insert all PuzzleController to PuzzleManager
            puzzleManager.Construct(allPuzzleController);
        }

        private void LoadImageController()
        {
            IImageController imageController = _imageControllerGameObject.GetComponent<IImageController>();
            string rootImgFolderPath = $"/Resources/{EnvironmentData.Instance.PuzzleImagesRootFolder}/";
            // Each index mean each step. Example imagePathLists[0] mean image for Step[0].
            string[][] imagePathLists = new string[_missionConfig.MissionDetail.Length][];

            for (int i = 0; i < _missionConfig.MissionDetail.Length; i++)

            {
                StepDetail stepDetail = _missionConfig.MissionDetail[i];

                if(stepDetail.ImgDetail != null)
                {
                    if (stepDetail.ImgDetail.ImgList.Length == 0)
                    {
                        DirectoryInfo di = new DirectoryInfo(Application.dataPath + rootImgFolderPath + stepDetail.ImgDetail.ImgFolder);

                        // Check if image path is correct.
                        try {
                            if (di.Exists)
                            {
                                FileInfo[] images = di.GetFiles("*.png");
                                imagePathLists[i] = images.Select(x => x.FullName).ToArray();

                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogWarning("Image folder path is not correct.");
                            Debug.LogWarning("Image folder path: " + di.FullName);
                            Debug.LogWarning("Because: " + e.Message);
                        }
                        
                    }
                    else
                    {
                        string[] imagePaths = stepDetail.ImgDetail.ImgList.Select(x => Application.dataPath + rootImgFolderPath + stepDetail.ImgDetail.ImgFolder + "/" + x).ToArray();
                        imagePathLists[i] = new string[imagePaths.Length];
                        for (int j = 0; i < imagePaths.Length; j++) 
                        {
                            string imagePath = imagePaths[j];
                            if (!File.Exists(imagePath))
                            {
                                Debug.LogWarning("Image path is not correct.");
                                Debug.LogWarning("Image path: " + imagePath);
                            }
                            else
                            {
                                imagePathLists[i][0] = imagePath;
                            }
                        }
                    }

                }
            }

            imageController.SetImagesList(imagePathLists);
        }

        private void _InitiateMissionController()
        {
            MissionController missioncontroller = _missionControllerGameObject.GetComponent<MissionController>();
            missioncontroller.Initiate(_missionSceneData.MissionConfigFolderFullPath, _missionConfig.MissionName, _missionConfig.MissionDependTos, _missionConfig.MissionType, new SaveManager.SaveManager(), _missionSceneData.IsPassed);
        }
        #endregion

        // Use this for initialization
        void Start()
        {
            StartGenerating();
            //Start gameplay after mission generation.
            _gameplayManagerGameObjefct.GetComponent<IGameplayManager>().StartGameplay();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}