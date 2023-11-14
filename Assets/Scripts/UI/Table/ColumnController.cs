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

    public class ColumnController : MonoBehaviour, IColumnController
    {
        [Header("Cell configuration")]
        [SerializeField] private GameObject _cellPrefab;
        [SerializeField] private int _defualtCellHeight = 30;
        [SerializeField] private int _cellFontSize = 16;
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
            foreach (var cellText in data)
            {
                GameObject cellRef = Instantiate(_cellPrefab, this.transform);
                setCell(cellRef, cellText);
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

