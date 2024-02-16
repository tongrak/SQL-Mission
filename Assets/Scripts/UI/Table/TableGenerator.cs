using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.Table
{
    interface ITableGenerator
    {
        void setTable(string[][] data);
    }

    public class GenericGenerator : GameplayController
    {
        protected void detroyExistedChilds()
        {
            foreach (Transform child in this.transform) { Destroy(child.gameObject); }
        }
    }

    public class TableGenerator : GenericGenerator, ITableGenerator
    {
        [Header("Column Configuration")]
        [SerializeField] private GameObject _columnPrefab;


        public void setTable(string[][] data)
        {
            detroyExistedChilds();

            foreach (var col in data)
            {
                GameObject colRef = Instantiate(_columnPrefab, this.transform);
                colRef.GetComponent<ICellsGenerator>().setCellsDisplay(col);
            }
        }
    }
}
