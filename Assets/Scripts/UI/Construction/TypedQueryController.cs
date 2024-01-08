using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.Construction
{
    public class TypedQueryController : GameplayController, ITypedQuery
    {
        [Header("UI Components")]
        [SerializeField] private GameObject _inputGO;

        private TMP_InputField _inputField => mustGetComponent<TMP_InputField>(_inputGO);
        public void updateQueryString(string s) => this._currentQuery = s;
        private string _currentQuery;

        public void startConsole()
        {
            _inputField.text = string.Empty;
            updateQueryString(string.Empty);
        }

        public string queryString => _currentQuery;
    }
}


