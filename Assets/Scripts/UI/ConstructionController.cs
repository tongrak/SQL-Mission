using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IContructionTabController
    {
        string queryString { get; }
    }

    public class ConstructionController : MonoBehaviour, IContructionTabController
    {
        [SerializeField] private string _defaultQuery;
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
    }
}


