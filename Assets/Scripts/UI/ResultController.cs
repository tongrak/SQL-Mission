using Assets.Scripts.PuzzleComponent;
using Gameplay.UI.Table;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI
{
    public interface IResultTabController
    {
        void setDisplayResult(ExecuteResult result);
    }

    public class ResultController : MonoBehaviour, IResultTabController
    {
        [SerializeField] private GameObject _tableGenerator;
        private ITableController _tableController
        {
            get
            {
                var contr = _tableGenerator.GetComponent<ITableController>();
                if (contr != null) return contr;
                else throw new System.Exception("Cannot get table controller");
            }
        }

        public void setDisplayResult(ExecuteResult result)
        {
            //TODO: Check error
            _tableController.setTable(result.TableResult);
            throw new System.NotImplementedException();
        }

    }
}


