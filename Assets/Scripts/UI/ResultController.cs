using Assets.Scripts.BackendComponent;
using Gameplay.UI.Table;
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
            _tableGenerator.SetActive(true);
            _tableController.setTable(result.TableResult);
        }

    }
}


