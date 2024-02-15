using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Gameplay.UI.Table;
using System;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IResultTabController
    {
        void setDisplayResult(ExecuteResult executeResult, PuzzleResult puzzleResult);
    }

    public class ResultConsoleController : GameplayController, IResultTabController
    {
        [Header("Table generator")]
        [SerializeField] private GameObject _tableGenerator;
        [SerializeField] private GameObject _feedbackGO;
        private ITableController _tableController => mustGetComponent<ITableController>(_tableGenerator);
        private IResultFeedbackController _feedbackController => mustGetComponent<IResultFeedbackController>(_feedbackGO);

        [Header("UI component")]
        [SerializeField] private GameObject _errorText;
        private TextMeshProUGUI _errorTextMesh => mustGetComponent<TextMeshProUGUI>(_errorText);

        private void generateTableResult(string[][] tableData)
        {
            //Filter out image paths
            _tableGenerator.SetActive(true);
            string[][] displayResult = tableData;
            if (tableData[0][0].ToLower().Equals("image"))
            {
                string[][] rawResult = tableData;
                int displayLegth = rawResult.Length - 1;
                displayResult = new string[displayLegth][];
                Array.Copy(rawResult, 1, displayResult, 0, displayLegth);
            }

            _tableGenerator.SetActive(true);
            _tableController.setTable(displayResult);
        }

        public void setDisplayResult(ExecuteResult executeResult, PuzzleResult puzzleResult)
        {
            _tableGenerator.SetActive(false);
            _errorText.SetActive(false);
            _feedbackGO.SetActive(true);

            if (executeResult.IsError)
            {
                _errorText.SetActive(true);
                _errorTextMesh.text = executeResult.ErrorMessage;
                _feedbackController.DisplayErrorFeedback();
                return;
            }

            if (puzzleResult.IsCorrect) _feedbackController.DisplayCorrectFeedback();
            else _feedbackController.DisplayIncorrectFeedback(puzzleResult.Reason);

            generateTableResult(executeResult.TableResult);
        }

    }
}


