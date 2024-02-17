using System.Collections;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IGameplayStepDisplayController
    {
        void UpdateStepTraverseState(bool canTraverseForward, bool canTraverseBackward);
    }
    public class GameplayStepDisplayController : GameplayController, IGameplayStepDisplayController
    {
        [Header("Traverse Button Obeject")]
        [SerializeField] private GameObject _forwardButtonGO;
        [SerializeField] private GameObject _backwardButtonGO;

        InactivableButtonScript _forwardButton => mustGetComponent<InactivableButtonScript>(_forwardButtonGO);
        InactivableButtonScript _backwardButton => mustGetComponent<InactivableButtonScript>(_backwardButtonGO);

        public void UpdateStepTraverseState(bool canTraverseForward, bool canTraverseBackward)
        {
            _forwardButtonGO.SetActive(true);
            _backwardButtonGO.SetActive(true);

            _forwardButton.SetButtonActive(canTraverseForward, false);
            _backwardButton.SetButtonActive(canTraverseBackward, false);
        }
    }
}