using UnityEngine;

namespace Assets.Scripts.PuzzleComponent.StepComponent
{
    public class StepController : MonoBehaviour, IStepController
    {
        private GameStep[] _allGameStep;
        private int _gameStepIndex = 0;

        public GameStep ChangeStep()
        {
            _gameStepIndex++;
            return _allGameStep[_gameStepIndex];
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
    }
}
