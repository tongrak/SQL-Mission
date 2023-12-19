using System.Collections;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IConsoleTabsController
    {
        void SetTab(TabType tab);
    }

    public class TabsButtonController : GameplayController, IConsoleTabsController
    {
        [Header("Temporary config")]
        [SerializeField] private UnityEngine.Color _activeColor;
        [SerializeField] private UnityEngine.Color _inactiveColor;

        [Header("Button gameobject")]
        [SerializeField] private GameObject _constructionGameobject;
        [SerializeField] private GameObject _resultGameobject;

        private UnityEngine.UI.Image _constructionImage => mustGetComponent<UnityEngine.UI.Image>(_constructionGameobject);
        private UnityEngine.UI.Image _resultImage => mustGetComponent<UnityEngine.UI.Image>(_resultGameobject);

        public void SetTab(TabType tab)
        {
            _constructionImage.color = _inactiveColor;
            _resultImage.color = _inactiveColor;
            switch (tab)
            {
                case TabType.CONSTRUCT: _constructionImage.color = _activeColor; break;
                case TabType.RESULT: _resultImage.color = _activeColor; break; 
            }
        }
    }
}