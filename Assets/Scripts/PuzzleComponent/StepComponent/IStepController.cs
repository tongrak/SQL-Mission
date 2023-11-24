namespace Assets.Scripts.PuzzleComponent.StepComponent
{
    public interface IStepController
    {
        void SetAllGameStep(GameStep[] allGameStep);
        /// <summary>
        /// Change to next step and get that step
        /// </summary>
        /// <returns>step after change</returns>
        GameStep ChangeStep();
        /// <summary>
        /// Get next step
        /// </summary>
        GameStep GetNextStep();
        GameStep GetCurrentStep();
    }
}