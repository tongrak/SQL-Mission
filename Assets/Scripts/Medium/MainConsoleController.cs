using Assets.Scripts.PuzzleComponent;
using Assets.Scripts.PuzzleComponent.StepComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    interface IPCMedium
    {
        ExecuteResult getResult(string query);
    }

    enum TabType
    {
        CONSTRUCT,
        RESULT
    }
}

namespace Gameplay.UI
{
    interface IMainConsoleController
    {
        void setDisplayTab(TabType tabType);

        void clickExecute();
    }


    public class MainConsoleController : MonoBehaviour, IMainConsoleController, IPCMedium
    {
        [Header("Tab Configure")]
        [SerializeField] private GameObject _constructionTab;
        [SerializeField] private GameObject _resultTab;

        private IContructionTabController _constrCon
        {
            get
            {
                var contr = _constructionTab.GetComponent<IContructionTabController>();
                if (contr != null) return contr;
                else throw new System.Exception("Cannot get contruction tab controller");
            }
        }
        private IResultTabController _resultCon
        {
            get
            {
                var contr = _resultTab.GetComponent<IResultTabController>();
                if (contr != null) return contr;
                else throw new System.Exception("Cannot get result tab controller");
            }
        }

        private IPuzzleController _currPC;
        private ExecuteResult _currExeResult;

        public void clickExecute()
        {
            Debug.Log("Query:" + _constrCon.queryString);
            _currExeResult = getResult(_constrCon.queryString);
            Debug.Log(_currExeResult);
            _resultCon.setDisplayResult(_currExeResult);
            ((IMainConsoleController)this).setDisplayTab(TabType.RESULT);
        }

        public ExecuteResult getResult(string query) => _currPC.GetExecuteResult(query);

        void IMainConsoleController.setDisplayTab(TabType tabType)
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

        //TODO: aquire PC .. somehow
        //temp solution
        private void Start()
        {
            this._currPC = GetComponent<IPuzzleController>();
            ((IMainConsoleController)this).setDisplayTab(TabType.CONSTRUCT);
            
        }
    }


}

