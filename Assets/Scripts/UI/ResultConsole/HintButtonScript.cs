using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IHintButtonController
    {
        void SetToInitState();
    }

    public class HintButtonScript : InactivableButtonScript, IHintButtonController
    {
        private bool _isHintActive = false;

        public void OnClickUpdateButton()
        {
            this.SetButtonActive(_isHintActive, true);
            _isHintActive = !_isHintActive;
        }

        public void SetToInitState()
        {
            this.SetButtonActive(true, true);
            _isHintActive = false;
        }
    }
}


