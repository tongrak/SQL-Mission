using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Step GetNextStep()
        {
            return _allGameStep[_gameStepIndex + 1].CurrStep;
        }
    }
}
