using Gameplay.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI { 
    interface ITextBox
    {
        string displayedText { get; set; }
    }
}

public class DialogBoxController : MonoBehaviour, ITextBox
{
    [SerializeField] private bool _displayDefaultText = false;
    [SerializeField] private string _defeatText = "Simple text";
    private string _currText = string.Empty;
    [SerializeField] private int _maxCharsDisplay = 1000;
    //[SerializeField] private Scrollbar _scrollBar;
    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField] private RectTransform _displayedZoneRect;
    [SerializeField] private RectTransform _currRectTransform;

    public string displayedText {
        get { return _currText; }
        set
        {
            if (value.Length > _maxCharsDisplay)
            {
                Debug.LogWarning("Given text exsee configure max chars(" + _maxCharsDisplay + ") ");
                return;
            }

            _currText = value;
            _textComponent.text = value;
            setHeight();
        }
    }

    private void setHeight() => _currRectTransform.sizeDelta = new Vector2(_currRectTransform.sizeDelta.x, _textComponent.preferredHeight);

    void Start()
    {
       if (_displayDefaultText) this.displayedText = _defeatText;
    }
}
