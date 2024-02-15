using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IResultFeedbackController
    {
        void DisplayCorrectFeedback();
        void DisplayIncorrectFeedback(string message);
        void DisplayErrorFeedback();
    }
    public class ResultFeedbackController : GameplayController, IResultFeedbackController
    {
        [Header("Gameobjects")]
        [SerializeField] private GameObject _feedbackTextGO;

        [Header("Sprites")]
        [SerializeField] private Sprite _errorFeedback;
        [SerializeField] private Sprite _incorrectFeedback;
        [SerializeField] private Sprite _correctFeedback;

        UnityEngine.UI.Image _bodyImage => mustGetComponent<UnityEngine.UI.Image>(this.gameObject);
        TMPro.TextMeshProUGUI _feedbackTextMesh => mustGetComponent<TMPro.TextMeshProUGUI>(_feedbackTextGO);

        public void DisplayCorrectFeedback()
        {
            this.gameObject.SetActive(true);
            _feedbackTextGO.SetActive(false);
            _bodyImage.sprite = _correctFeedback;
        }
        public void DisplayErrorFeedback()
        {
            this.gameObject.SetActive(true);
            _feedbackTextGO.SetActive(false);
            _bodyImage.sprite = _errorFeedback;
        }
        public void DisplayIncorrectFeedback(string message)
        {
            this.gameObject.SetActive(true);
            _feedbackTextGO.SetActive(true);
            _feedbackTextMesh.text = message;
            _bodyImage.sprite = _incorrectFeedback;
        }

        #region Unity Basic
        private void Awake()
        {
            this.DisplayIncorrectFeedback("Please execute for a result");
        }
        #endregion
    }
}


