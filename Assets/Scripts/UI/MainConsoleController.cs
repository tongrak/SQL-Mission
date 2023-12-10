using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.StepComponent;
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

    public abstract class GameplayController : MonoBehaviour
    {
        /// <summary>
        /// Get a first component of given or child of the given
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interestedGO"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected T mustGetComponent<T>(GameObject interestedGO)
        {
            T contr = interestedGO.GetComponent<T>();
            if (contr != null) return contr;
            contr = interestedGO.GetComponentInChildren<T>();
            if (contr != null) return contr;
            throw new System.Exception("Cannot get component with type(" + nameof(T) + ")");
        }
        /// <summary>
        /// Get a first component of the object with a given name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        protected T mustFindComponentOfName<T>(string name)
        {
            var contrGO = GameObject.Find(name);
            if (contrGO == null) throw new System.Exception("Cannot get component of null gammobject with name:" + name);
            return mustGetComponent<T>(contrGO);
        }

        public abstract void activateController();
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

        public override void activateController() => Debug.Log("Activate main console");

        public string getCurrentQueryString() => _constrCon.queryString;
        public void setResultDisplay(bool isPass, ExecuteResult result)
        {
            setDisplayTab(TabType.RESULT);
            if (isPass) _constrCon.clearQueryString(); 
            _resultCon.setDisplayResult(isPass, result);
        }
        public void setToConstructTab() => setDisplayTab(TabType.CONSTRUCT);
        public void setToResultTab() => setDisplayTab(TabType.RESULT);
    }
}

