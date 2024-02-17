using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public class InactivableButtonScript : GameplayController
    {
        [Header("Activity sprites")]
        [SerializeField] protected Sprite _activeSprite;
        [SerializeField] protected Sprite _inactiveSprite;

        public void SetButtonActive(bool active, bool isInactiveInteractable)
        {
            this.gameObject.GetComponent<UnityEngine.UI.Image>().sprite = active ? _activeSprite : _inactiveSprite;
            this.gameObject.GetComponent<UnityEngine.UI.Button>().interactable = isInactiveInteractable || active;
        }
    }
}


