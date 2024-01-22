using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.DataPersistence.StepController
{
    public class StepController : MonoBehaviour, IStepController
    {
        public UnityEvent OnAllStepPassed { get; private set; }
        public GameStep[] AllGameStep { get; private set; }
        private int _gameStepIndex = 0;

        public void ChangeStep()
        {
            _gameStepIndex++;
            if (AllGameStep[_gameStepIndex].CurrStep == Step.EndStep) { OnAllStepPassed?.Invoke(); }
        }

        public GameStep GetCurrentStep()
        {
            return AllGameStep[_gameStepIndex];
        }

        public GameStep GetNextStep()
        {
            return AllGameStep[_gameStepIndex + 1];
        }

        public void SetAllGameStep(GameStep[] allGameStep)
        {
            AllGameStep = allGameStep;
        }

        private void Awake()
        {
            if (OnAllStepPassed == null)
            {
                OnAllStepPassed = new UnityEvent();
            }
        }

        private void OnDisable()
        {
            OnAllStepPassed?.RemoveAllListeners();
        }
    }
}
