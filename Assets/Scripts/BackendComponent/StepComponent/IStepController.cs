namespace Assets.Scripts.BackendComponent.Model
{
    public interface IStepController
    {
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
    }
}