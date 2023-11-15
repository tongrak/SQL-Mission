using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent.StepComponent
{
    public struct GameStep
    {
        public readonly Step CurrStep;
        public readonly int DialogIndex; // If current step isn't "Dialog", DialogIndex must be "-1"
        public readonly int PCIndex; // If current step isn't "Puzzle", PCIndex must be "-1"

        public GameStep(Step step, int dialogIndex, int PCIndex)
        {
            this.CurrStep = step;
            this.DialogIndex = dialogIndex;
            this.PCIndex = PCIndex;
        }
    }
}
