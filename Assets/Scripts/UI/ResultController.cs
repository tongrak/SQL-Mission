using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.Model;
using Gameplay.UI.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IResultTabController
    {
        void setDisplayResult(bool isPass, ExecuteResult result);
    }

    public class ResultController : GameplayController, IResultTabController
    {
        [Header("Table generator")]
        [SerializeField] private GameObject _tableGenerator;
        private ITableController _tableController => mustGetComponent<ITableController>(_tableGenerator);

        [Header("UI component")]
        [SerializeField] private GameObject _proceedButton;
        [SerializeField] private GameObject _errorText;
        private TextMeshProUGUI _errorTextMesh => mustGetComponent<TextMeshProUGUI>(_errorText);

        private bool _isPass;

        public void setDisplayResult(bool isPass, ExecuteResult result)
        {
            if (!_isPass) _isPass = isPass;

            _tableGenerator.SetActive(false);
            _errorText.SetActive(false);

            if (result.IsError)
            {
                _errorTextMesh.text = result.ErrorMessage;
                _errorText.SetActive(true);
                return;
            }
            //Filter out image paths
            string[][] rawResult = result.TableResult;
            int displayLegth = rawResult.Length-1;
            string[][] displayResult = new string[displayLegth][];
            Array.Copy(rawResult,1,displayResult,0,displayLegth);

            _tableGenerator.SetActive(true);
            _tableController.setTable(displayResult);
        }

    }
}


