using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.UI.Table
{
    interface ITableCont
    {
        void setTable(string[][] data);
    }

    public class TableController : MonoBehaviour, ITableCont
    {
        [Header("Column Configuration")]
        [SerializeField] private GameObject _columnPrefab;
        [SerializeField] private string[][] _tempData;
        private void deleteExistedChilds()
        {
            foreach (Transform child in this.transform) { Destroy(child.gameObject); }
        }

        public void setTable(string[][] data)
        {
            deleteExistedChilds();

            foreach (var col in data)
            {
                GameObject colRef = Instantiate(_columnPrefab, this.transform);
                var colContr = colRef.GetComponent<IColumnController>();
                colContr.setColumnDisplay(col);
            }
        }
    }
}
