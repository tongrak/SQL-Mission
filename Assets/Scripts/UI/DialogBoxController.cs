using Gameplay.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI { 
    interface IDialogBox
    {
        
        void setDisplayedText(string dialog);
    }
}

public class DialogBoxController : MonoBehaviour, IDialogBox
{
    [SerializeField]
    private bool _dislayDefeatText = false;
    [SerializeField]
    private string _defeatText = "Simple text";
    [SerializeField]
    private int _maxCharsDisplay = 1000;
    [SerializeField]
    private Scrollbar _scrollBar;
    [SerializeField]
    private TextMeshProUGUI _textComponent;
    [SerializeField]
    private RectTransform _displayedZoneRect;
    [SerializeField]
    private RectTransform _currRectTransform;

    public void setDisplayedText(string dialog)
    {
        if (dialog.Length > _maxCharsDisplay) {
            Debug.LogWarning(dialog + " : have more chars than configured max (" + _maxCharsDisplay + ") ");
            return;
        }

        _textComponent.text = dialog;
        setHeight();
        setScrollable();
    }

    private void setHeight() => _currRectTransform.sizeDelta = new Vector2(_currRectTransform.sizeDelta.x, _textComponent.preferredHeight);

    private void setScrollable()
    {
        if (_currRectTransform.sizeDelta.y > _displayedZoneRect.sizeDelta.y) _scrollBar.interactable = false;
        else _scrollBar.interactable = true;
    }

    //private set

    void Start()
    {
       if (_dislayDefeatText) setDisplayedText(_defeatText);
       setHeight();
       setScrollable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
