using Assets.Scripts.MissionGenComponent.Model;
using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.BlankBlockComponent;
using Assets.Scripts.BackendComponent.DialogController;
using Assets.Scripts.BackendComponent.SQLComponent;
using Assets.Scripts.BackendComponent.StepComponent;
using System.Linq;
using UnityEngine;
using Assets.Scripts.BackendComponent.ImageController;
using System.IO;
using Assets.Scripts.ScriptableObjects;

namespace Assets.Scripts.MissionGenComponent
{
    public class MissionGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogConGameObject;
        [SerializeField] private GameObject _stepControllerGameObject;
        [SerializeField] private GameObject _puzzleManagerGameObject;
        [SerializeField] private GameObject _imageControllerGameObject;
        [SerializeField] private MissionData _missionData;

        private MissionConfig _missionConfig;
        private ISQLService _sqlService = new SQLService();
        private IFixedTemplateService _fixedTemplateService = new FixedTemplateService();
        private IUpToConfigTemplateService _upToConfigTemplateService;

        private void StartGenerating()
        {
            _upToConfigTemplateService = new UpToConfigTemplateService(_sqlService);

            LoadConfigFile();
            LoadDialogController();
            LoadStepController();
            LoadPuzzleManager();
            LoadImageController();
        }

        private void LoadConfigFile()
        {
            TextAsset missionConfigFile = Resources.Load<TextAsset>(_missionData.missionConfigFolderPath + "/" + _missionData.missionFileName);
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
            GameStep[] allGameStep = new GameStep[allConfigStep.Length + 1];
            for(int i = 0; i < allConfigStep.Length; i++)
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
            StepDetail[] allStepDetail = _missionConfig.MissionDetail.Where(x => x.Step == Step.Puzzle).ToArray();
            PuzzleController[] allPuzzleController = new PuzzleController[allStepDetail.Length];

            for(int i = 0; i < allStepDetail.Length; i++)
            {
                StepDetail stepDetail = allStepDetail[i];
                // 1) Create database path
                string dbFolder = "/Resources/Database/";
                string dbConn = "URI=file:" + Application.dataPath + dbFolder + stepDetail.PuzzleDetail.DB;
                // 2) Get schema from SQLService
                Schema[] schemas = _sqlService.GetSchemas(dbConn, stepDetail.PuzzleDetail.Tables);
                // 3) Create PuzzleController
                PuzzleController puzzleController = new PuzzleController(dbConn, stepDetail.PuzzleDetail.AnswerSQL, stepDetail.Dialog, schemas, _sqlService, stepDetail.PuzzleDetail.VisualType, _fixedTemplateService, _upToConfigTemplateService, stepDetail.PuzzleDetail.SpecialBlankOptions, i == allStepDetail.Length - 1);
                // 4) Insert PuzzleController to array.
                allPuzzleController[i] = puzzleController;

            }
            // 5) Insert all PuzzleController to PuzzleManager
            puzzleManager.SetAllPC(allPuzzleController);
        }

        private void LoadImageController()
        {
            IImageController imageController = _imageControllerGameObject.GetComponent<IImageController>();
            string rootImgFolderPath = "/Resources/PuzzleImages/";
            string[][] imagePathLists = new string[_missionConfig.MissionDetail.Length][];

            for (int i = 0; i < _missionConfig.MissionDetail.Length; i++)

            {
                StepDetail stepDetail = _missionConfig.MissionDetail[i];

                if(stepDetail.ImgDetail != null)
                {
                    if (stepDetail.ImgDetail.ImgList.Length == 0)
                    {
                        FileInfo[] images = new DirectoryInfo(Application.dataPath + rootImgFolderPath + stepDetail.ImgDetail.ImgFolder).GetFiles("*.png");
                        imagePathLists[i] = images.Select(x => x.FullName).ToArray();
                    }
                    else
                    {
                        imagePathLists[i] = stepDetail.ImgDetail.ImgList.Select(x => Application.dataPath + rootImgFolderPath + stepDetail.ImgDetail.ImgFolder + "/" + x).ToArray();
                    }

                }
            }

            imageController.SetImagesList(imagePathLists);
        }

        // Use this for initialization
        void Start()
        {
            StartGenerating();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}