namespace Assets.Scripts.DataPersistence.StepController
{
    public struct GameStep
    {
        public readonly Step CurrStep;
        public readonly int StepIndex;
        public readonly int DialogIndex; // If current step isn't "Dialog", DialogIndex must be "-1"
        public readonly int PCIndex; // If current step isn't "Puzzle", PCIndex must be "-1"

        public GameStep(Step step, int stepIndex, int dialogIndex, int pcIndex)
        {
            this.CurrStep = step;
            this.StepIndex = stepIndex;
            this.DialogIndex = dialogIndex;
            this.PCIndex = pcIndex;
        }
    }
}
