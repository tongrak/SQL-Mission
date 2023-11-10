using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent.StepComponent
{
    public interface IStepController
    {
        /// <summary>
        /// Change to next step and get that step
        /// </summary>
        /// <returns>step after change</returns>
        GameStep ChangeStep();
        /// <summary>
        /// Get next step
        /// </summary>
        Step GetNextStep();
    }
}
