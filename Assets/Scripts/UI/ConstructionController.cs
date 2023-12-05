using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IContructionTabController
    {
        string queryString { get; }
        void clearQueryString();
    }

    public class ConstructionController : MonoBehaviour, IContructionTabController
    {
        [Header("Query text configuration")]
        [SerializeField] private string _defaultQuery;

        [Header("Input configuration")]
        [SerializeField] private TextMeshProUGUI _queryTextMesh;

        private string _query;
        public string queryString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_query)) _query = _defaultQuery;
                return _query;
            }

            set { _query = value; }
        }

        public void clearQueryString() => _queryTextMesh.text = "";
    }
}


