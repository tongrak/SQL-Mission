using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IHintButtonController
    {
        void SetToInitState();
    }

    public class HintButtonScript : MonoBehaviour, IHintButtonController
    {
        [Header("Sprite")]
        [SerializeField] private Sprite _activeHintSprite;
        [SerializeField] private Sprite _deactiveHintSprite;

        private bool _isActiveHint = false;

        public void OnClickUpdateButton()
        {
            this.GetComponent<UnityEngine.UI.Image>().sprite = _isActiveHint ? _activeHintSprite : _deactiveHintSprite;
            _isActiveHint = !_isActiveHint;
        }

        public void SetToInitState()
        {
            this.GetComponent<UnityEngine.UI.Image>().sprite = _activeHintSprite;
            _isActiveHint = false;
        }
    }
}


