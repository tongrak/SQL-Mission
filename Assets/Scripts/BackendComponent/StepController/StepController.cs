using UnityEngine;

namespace Assets.Scripts.BackendComponent.StepController
{
    public class StepController : MonoBehaviour, IStepController
    {
        [SerializeField] private MissionController _missionController;
        private GameStep[] _allGameStep;
        private int _gameStepIndex = 0;

        public void ChangeStep()
        {
            _gameStepIndex++;
            if (_allGameStep[_gameStepIndex].CurrStep == Step.EndStep) { OnAllStepPassed(); }
        }

        public GameStep GetCurrentStep()
        {
            return _allGameStep[_gameStepIndex];
        }

        public GameStep GetNextStep()
        {
            return _allGameStep[_gameStepIndex + 1];
        }

        public void SetAllGameStep(GameStep[] allGameStep)
        {
            _allGameStep = allGameStep;
        }

        private void OnAllStepPassed()
        {
            _missionController.AllPuzzlePassed();
        }
    }
}
