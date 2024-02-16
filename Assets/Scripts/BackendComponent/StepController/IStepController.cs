using UnityEngine.Events;

namespace Assets.Scripts.DataPersistence.StepController
{
    public interface IStepController
    {
        UnityEvent OnAllStepPassed { get; }
        GameStep[] AllGameStep { get; }
        void SetAllGameStep(GameStep[] allGameStep);
        /// <summary>
        /// Change to next step and get that step
        /// </summary>
        void ChangeStep();
        /// <summary>
        /// Get next step
        /// </summary>
        GameStep GetNextStep();
        GameStep GetCurrentStep();
        public void GoBackPreviousStep();
    }
}