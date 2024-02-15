using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Table
{
    interface IColumnController
    {
        int cellHeight { get; set; }
        void setColumnDisplay(string[] data);
    }

    public class ColumnController : GameplayController, IColumnController
    {
        [Header("Cell configuration")]
        [SerializeField] protected GameObject _cellPrefab;
        [SerializeField] private int _defualtCellHeight = 30;
        [SerializeField] private int _cellFontSize = 16;

        [Header("Sprite")]
        [SerializeField] private Sprite _tableHeader;

        private int _cellHeight = 0;

        private float _columnWidth = 0f;

        public int cellHeight
        {
            get { return (_cellHeight > 0) ? _cellHeight : _defualtCellHeight; }
            set
            {
                if (value <= 0)
                {
                    Debug.LogWarning("cell height(" + value + ") cannot be lower than 0");
                    return;
                }
                _cellHeight = value;
            }
        }

        public void setColumnDisplay(string[] data)
        {
            bool isHeader = true;
            foreach (var cellText in data)
            {
                GameObject cellRef = Instantiate(_cellPrefab, this.transform);
                setCell(cellRef, cellText);
                //Set the first first to Column cell
                if (isHeader)
                {
                    cellRef.GetComponent<UnityEngine.UI.Image>().sprite = _tableHeader;
                    isHeader = false;
                }
            }
            //set column height
           
            var columnRect = this.GetComponent<RectTransform>();
            columnRect.sizeDelta = new Vector2(columnRect.sizeDelta.x, cellHeight * data.Length);
        }

        private void setCell(GameObject cellObj, string cellText)
        {
            var cellCon = cellObj.GetComponent<ICellController>();
            var tmp = cellCon.TextMeshPro; 
            tmp.fontSize = _cellFontSize;
            tmp.text = cellText;

            if (_columnWidth == 0)
            {
                var columnRect = this.GetComponent<RectTransform>();
                columnRect.sizeDelta = new Vector2(tmp.preferredWidth, cellHeight);
                _columnWidth = tmp.preferredWidth;
            }

            var rect = cellObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(_columnWidth, cellHeight);
        }
    }
}

