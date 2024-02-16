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
        [SerializeField] private GameObject _queriedTableGeneratorGO;
        [SerializeField] private GameObject _expectedTableGeneratorGO;

        [Header("Feedback objects")]
        [SerializeField] private GameObject _feedbackGO;
        [SerializeField] private GameObject _hintButtonGO;
        [SerializeField] private GameObject _bannerGO;

        [Header("Banner sprite")]
        [SerializeField] private Sprite _queriedBannerSprite;
        [SerializeField] private Sprite _expectedBannerSprite;

        private ITableGenerator _queriedTable => mustGetComponent<ITableGenerator>(_queriedTableGeneratorGO);
        private ITableGenerator _expectedTable => mustGetComponent<ITableGenerator>(_expectedTableGeneratorGO);
        private IResultFeedbackController _feedbackController => mustGetComponent<IResultFeedbackController>(_feedbackGO);
        private IHintButtonController _hintButtonController => mustGetComponent<IHintButtonController>(_hintButtonGO);


        [Header("UI component")]
        [SerializeField] private GameObject _errorText;
        private TextMeshProUGUI _errorTextMesh => mustGetComponent<TextMeshProUGUI>(_errorText);

        private bool _isDisplayQueriedTable = true;

        private void generateTableResult(ITableGenerator targetGenerator, string[][] tableData)
        {
            //Filter out image paths
            string[][] displayResult = tableData;
            if (tableData[0][0].ToLower().Equals("image"))
            {
                string[][] rawResult = tableData;
                int displayLegth = rawResult.Length - 1;
                displayResult = new string[displayLegth][];
                Array.Copy(rawResult, 1, displayResult, 0, displayLegth);
            }

            targetGenerator.setTable(displayResult);
        }

        private void generateAllTable(string[][] queriedTable, string[][] expectedTable)
        {
            //generate table
            _queriedTableGeneratorGO.SetActive(true);
            generateTableResult(_queriedTable, queriedTable);
            _expectedTableGeneratorGO.SetActive(true);
            generateTableResult(_expectedTable, expectedTable);

            _isDisplayQueriedTable = true;
            _expectedTableGeneratorGO.SetActive(false);
        }

        public void setDisplayResult(ExecuteResult executeResult, PuzzleResult puzzleResult)
        {
            //init the console
            _queriedTableGeneratorGO.SetActive(false);
            _expectedTableGeneratorGO.SetActive(false);
            _hintButtonGO.SetActive(false);
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
            else
            {
                _feedbackController.DisplayIncorrectFeedback(puzzleResult.Reason);
                _hintButtonGO.SetActive(true);
                _hintButtonController.SetToInitState();
            }
            //TODO: request expected table
            generateAllTable(executeResult.TableResult, executeResult.TableResult);
        }

        public void OnClickHint()
        {
            _isDisplayQueriedTable = !_isDisplayQueriedTable;

            _queriedTableGeneratorGO.SetActive(_isDisplayQueriedTable);
            _expectedTableGeneratorGO.SetActive(!_isDisplayQueriedTable);
            _bannerGO.GetComponent<UnityEngine.UI.Image>().sprite = _isDisplayQueriedTable ? _queriedBannerSprite : _expectedBannerSprite;
        }

    }
}


