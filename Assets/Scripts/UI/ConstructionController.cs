using Gameplay.UI.Construction;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Construction
{
    public enum ConstructionType { TYPING, FILL_THE_BLANK }
    public interface IConstructionConsole
    {
        string queryString { get; }
    }
    public interface IFillTheBlankQuery : IConstructionConsole
    {
        void SetUpTokenField(string tokens);
    }
    public interface IExecuteOnlyQuery : IConstructionConsole 
    {
        void SetUpQuery(string query);
    }
    public interface ITypedQuery : IConstructionConsole{}
}

namespace Gameplay.UI
{
    public interface IContructionConsoleController
    {
        void SetContructionType(ConstructionType type);
        void clearQueryString();
        string queryString { get; }
    }

    public class ConstructionController : MonoBehaviour, IContructionConsoleController
    {
        [Header("Input Gameobjects")]
        [SerializeField] private GameObject _fillTheBlankGameobject;
        [SerializeField] private GameObject _typedQueryGameobject;

        [Header("Query text configuration")]
        [SerializeField] private string _defaultQuery;

        [Header("Input configuration")]
        [SerializeField] private TextMeshProUGUI _queryTextMesh;

        private ConstructionType _currentDisplayType;
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

        public void clearQueryString() => _queryTextMesh.text = string.Empty;

        public void SetContructionType(ConstructionType type)
        {
            throw new System.NotImplementedException();
        }
    }
}


