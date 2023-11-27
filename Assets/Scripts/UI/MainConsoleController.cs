using Assets.Scripts.PuzzleComponent;
using Assets.Scripts.PuzzleComponent.StepComponent;
using Gameplay.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public enum TabType
    {
        CONSTRUCT,
        RESULT
    }

    public class GameplayController : MonoBehaviour
    {
        protected T mustGetComponent<T>() => this.mustGetComponent<T>(this.gameObject);
        protected T mustGetComponent<T>(GameObject interestedGO)
        {
            var contr = interestedGO.GetComponent<T>();
            if (contr != null) return contr;
            else throw new System.Exception("Cannot get component with type(" + nameof(T) + ")");
        }
    }
}

namespace Gameplay.UI
{
    public interface IMainConsoleController
    {
        /// <summary>
        /// Start puzzle solving sequence
        /// </summary>
        /// <param name="pc"></param>
        //void activatePuzzleSolving(IPuzzleController pc);
        /// <summary>
        /// Set a current display tab to given tab type
        /// </summary>
        /// <param name="tabType"></param>
        void setDisplayTab(TabType tabType);
        /// <summary>
        /// Set the console to construction tab
        /// </summary>
        void setToConstructTab();
        /// <summary>
        /// Set the console to result tab
        /// </summary>
        void setToResultTab();
        /// <summary>
        /// Set the result tab display according to given result
        /// </summary>
        /// <param name="result">interested result to be displayed</param>
        void setResultDisplay(bool isPass, ExecuteResult result);
        /// <summary>
        /// Get 
        /// </summary>
        /// <returns></returns>
        string getCurrentQueryString();
    }

    public class MainConsoleController : GameplayController, IMainConsoleController
    {
        #region UI Tab
        [Header("Tab Configure")]
        [SerializeField] private GameObject _constructionTab;
        [SerializeField] private GameObject _resultTab;

        private IContructionTabController _constrCon => mustGetComponent<IContructionTabController>(_constructionTab);
        private IResultTabController _resultCon => mustGetComponent<IResultTabController>(_resultTab);
        public void setDisplayTab(TabType tabType)
        {
            //disable all tab
            this._constructionTab.SetActive(false);
            this._resultTab.SetActive(false);

            switch (tabType)
            {
                case TabType.CONSTRUCT:
                    this._constructionTab.SetActive(true); break;
                case TabType.RESULT:
                    this._resultTab.SetActive(true); break;
                default: break;
            }
        }
        #endregion

        public string getCurrentQueryString() => _constrCon.queryString;
        //TODO:
        public void setResultDisplay(bool isPass, ExecuteResult result) => throw new System.NotImplementedException();

        public void setToConstructTab() => setDisplayTab(TabType.CONSTRUCT);
        public void setToResultTab() => setDisplayTab(TabType.RESULT);
    }
}

