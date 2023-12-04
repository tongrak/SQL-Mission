﻿using Assets.Scripts.MissionGenComponent.JSON_Class;
using Assets.Scripts.PuzzleComponent;
using Assets.Scripts.PuzzleComponent.BlankBlockComponent;
using Assets.Scripts.PuzzleComponent.SQLComponent;
using Assets.Scripts.PuzzleComponent.StepComponent;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MissionGenComponent
{
    public class MissionGenerator : MonoBehaviour
    {
        [SerializeField] private TextAsset _configFile;
        [SerializeField] private GameObject _dialogConGameObject;
        [SerializeField] private GameObject _stepControllerGameObject;
        [SerializeField] private GameObject _puzzleManagerGameObject;

        private MissionConfig _missionConfig;
        private ISQLService _sqlService = new SQLService();
        private IFixedTemplateService _fixTemplateService = new FixedTemplateService();
        private IUpToConfigTemplateService _upToConfigTemplateService;

        private void LoadConfigFile()
        {
            _missionConfig = JsonUtility.FromJson<MissionConfig>(_configFile.text);
        }

        private void LoadDialogController()
        {
            DialogController dialogController = _dialogConGameObject.GetComponent<DialogController>();
            dialogController.SetAllDialog(_missionConfig.MissionDetail.Where(x => x.Step == Step.Dialog).Select(x => x.Dialog).ToArray());
        }

        private void LoadStepController()
        {
            StepController stepController = _stepControllerGameObject.GetComponent<StepController>();
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
                        allGameStep[i] = new GameStep(Step.Dialog, dialogIndex, -1);
                        dialogIndex++;
                        break;
                    case Step.Puzzle:
                        allGameStep[i] = new GameStep(Step.Puzzle, -1, puzzleIndex);
                        puzzleIndex++;
                        break;
                    default:
                        break;
                }
            }
            allGameStep[allGameStep.Length - 1] = new GameStep(Step.EndStep, -1, -1);

            stepController.SetAllGameStep(allGameStep);
        }

        private void LoadPuzzleManager()
        {
            PuzzleManager puzzleManager = _puzzleManagerGameObject.GetComponent<PuzzleManager>();
            StepDetail[] allStepDetail = _missionConfig.MissionDetail.Where(x => x.Step == Step.Puzzle).ToArray();
            PuzzleController[] allPuzzleController = new PuzzleController[allStepDetail.Length];

            for(int i = 0; i < allStepDetail.Length; i++)
            {
                StepDetail stepDetail = allStepDetail[i];
                // 1) Create dbPath
                string dbFolder = "/Data/Database/";
                string dbConn = "URI=file:" + Application.dataPath + dbFolder + stepDetail.Detail.DB;
                // 2) Get schema from SQLService
                Schema[] schemas = _sqlService.GetSchemas(dbConn, stepDetail.Detail.Tables);
                // 3) Create PuzzleController
                PuzzleController puzzleController = new PuzzleController(dbConn, stepDetail.Detail.AnswerSQL, stepDetail.Dialog, schemas, _sqlService, stepDetail.Detail.ImgType, _fixTemplateService, _upToConfigTemplateService, stepDetail.Detail.SpecialBlankOptions);
                // 4) Insert PuzzleController to array.
                allPuzzleController[i] = puzzleController;

            }
            // 5) Insert all PuzzleController to PuzzleManager
            puzzleManager.SetAllPC(allPuzzleController);
        }

        // Use this for initialization
        void Start()
        {
            _upToConfigTemplateService = new UpToConfigTemplateService(_sqlService);
            LoadConfigFile();
            LoadDialogController();
            LoadStepController();
            LoadPuzzleManager();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}