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
        public readonly int DialogIndex;
        public readonly int PCIndex;

        public GameStep(Step step, int dialogIndex, int PCIndex)
        {
            this.CurrStep = step;
            this.DialogIndex = dialogIndex;
            this.PCIndex = PCIndex;
        }
    }
}
