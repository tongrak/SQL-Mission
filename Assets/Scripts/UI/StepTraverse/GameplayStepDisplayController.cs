using System.Collections;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IGameplayStepDisplayController
    {
        void UpdateStepTraverseState(bool canTraverseForward, bool canTraverseBackward);
        void UpdateStepTextDisplay(int currentStepIndex, int totalSteps);
    }
    public class GameplayStepDisplayController : GameplayController, IGameplayStepDisplayController
    {
        [Header("Traverse Button Obeject")]
        [SerializeField] private GameObject _forwardButtonGO;
        [SerializeField] private GameObject _backwardButtonGO;

        [Header("Text step display")]
        [SerializeField] private TextMeshProUGUI _currentStepText;
        [SerializeField] private TextMeshProUGUI _totalStepText;

        InactivableButtonScript _forwardButton => mustGetComponent<InactivableButtonScript>(_forwardButtonGO);
        InactivableButtonScript _backwardButton => mustGetComponent<InactivableButtonScript>(_backwardButtonGO);

        public void UpdateStepTextDisplay(int currentStepIndex, int totalSteps)
        {
            _currentStepText.gameObject.SetActive(true);
            _totalStepText.gameObject.SetActive(true);

            _currentStepText.text = currentStepIndex.ToString();
            _totalStepText.text = totalSteps.ToString();
        }

        public void UpdateStepTraverseState(bool canTraverseForward, bool canTraverseBackward)
        {
            _forwardButtonGO.SetActive(true);
            _backwardButtonGO.SetActive(true);

            _forwardButton.SetButtonActive(canTraverseForward, false);
            _backwardButton.SetButtonActive(canTraverseBackward, false);
        }
    }
}