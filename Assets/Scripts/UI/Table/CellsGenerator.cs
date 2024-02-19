using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Table
{
    interface ICellsGenerator
    {
        int cellHeight { get; set; }
        void setCellsDisplay(string[] data);
        void destroyAllCells();
    }

    public class CellsGenerator : GenericGenerator, ICellsGenerator
    {
        [Header("Cell configuration")]
        [SerializeField] protected GameObject _cellPrefab;
        [SerializeField] private int _defualtCellHeight = 30;
        [SerializeField] private float _widthBuffer = 4;
        [SerializeField] private int _cellFontSize = 16;

        [Header("Sprite")]
        [SerializeField] protected Sprite _columnHeaderSprite;
        [SerializeField] protected Sprite _columnBodySprite;

        //Runtime variables
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

        public void setCellsDisplay(string[] data)
        {
            detroyExistedChilds();

            bool isHeader = true;
            string longestText = data.OrderByDescending(x => x.Length).First();
            TextMeshProUGUI longestTextMesh = null ;
            foreach (var cellText in data)
            {
                GameObject cellRef = Instantiate(_cellPrefab, this.transform);
                setCell(cellRef, cellText);
                //Set cell sprite
                cellRef.GetComponent<UnityEngine.UI.Image>().sprite = isHeader ? _columnHeaderSprite : _columnBodySprite;
                if (isHeader) isHeader = false;
                if (cellText.Equals(longestText)) longestTextMesh = cellRef.GetComponent<TextMeshProUGUI>();
            }
            //set column height
           
            var columnRect = this.GetComponent<RectTransform>();
            float longestWidth = (longestTextMesh == null) ? columnRect.sizeDelta.x : longestTextMesh.preferredWidth;
            columnRect.sizeDelta = new Vector2(longestWidth + _widthBuffer, cellHeight * data.Length);
        }
        public void destroyAllCells() => base.detroyExistedChilds();
        private void setCell(GameObject cellObj, string cellText)
        {
            var cellCon = cellObj.GetComponent<ICellController>();
            var tmp = cellCon.TextMeshPro; 
            tmp.fontSize = _cellFontSize;
            tmp.text = cellText;

            if (tmp.preferredWidth > _columnWidth)
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

