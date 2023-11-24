using Assets.Scripts.MissionGenComponent.JSON_Class;
using Assets.Scripts.PuzzleComponent;
using Assets.Scripts.PuzzleComponent.SQLComponent;
using Assets.Scripts.PuzzleComponent.StepComponent;
using System.Collections;
using System.Collections.Generic;
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
            IEnumerable<Step> allConfigStep = _missionConfig.MissionDetail.Select(x => x.Step);

            int dialogIndex = 0;
            int puzzleIndex = 0;
            GameStep[] allGameStep = new GameStep[allConfigStep.Count() + 1];
            foreach (Step step in allConfigStep)
            {
                if (step == Step.Dialog)
                {
                    allGameStep.Append(new GameStep(Step.Dialog, dialogIndex, -1));
                    dialogIndex++;
                }
                else if (step == Step.Puzzle)
                {
                    allGameStep.Append(new GameStep(Step.Puzzle, -1, puzzleIndex));
                    puzzleIndex++;
                }
            }
            allGameStep.Append(new GameStep(Step.EndStep, -1, -1));

            stepController.SetAllGameStep(allGameStep);
        }

        private void LoadPuzzleManager()
        {
            PuzzleManager puzzleManager = _puzzleManagerGameObject.GetComponent<PuzzleManager>();
            IEnumerable<StepDetail> allStepDetail = _missionConfig.MissionDetail.Where(x => x.Step == Step.Puzzle);
            PuzzleController[] allPuzzleController = new PuzzleController[allStepDetail.Count()];

            foreach (StepDetail stepDetail in allStepDetail)
            {
                // 1) Create dbPath
                string mockDBfolder = "/Data/Database/";
                string dbConn = "URI=file:" + Application.dataPath + mockDBfolder + stepDetail.Detail.DB;
                // 2) Get schema from SQLService
                Schema[] schemas = _sqlService.GetSchemas(dbConn, stepDetail.Detail.Tables);
                // 3) Create PuzzleController
                PuzzleController puzzleController = new PuzzleController(dbConn, stepDetail.Detail.AnswerSQL, stepDetail.Dialog, schemas, _sqlService, stepDetail.Detail.ImgType);
            }
            // 4) Insert all PuzzleController to PuzzleManager
            puzzleManager.SetAllPC(allPuzzleController);
        }

        // Use this for initialization
        void Start()
        {
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