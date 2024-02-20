using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Gameplay.UI.Construction.FTB
{
    interface ILineTokenController
    {
        void SetUpLineToken((string, FillTheBlankToken)[] tokens);
    }
    public class LineTokenController : GameplayController, ILineTokenController
    {
        [Header("Token Prefabs")]
        [SerializeField] private GameObject _stringTokenPrefab;
        [SerializeField] private GameObject _InputTokenPrefab;

        [Header("Token configure")]
        [SerializeField] private int tokenHeight = 40;

        public void SetUpLineToken((string, FillTheBlankToken)[] tokens)
        {
            foreach (var token in tokens)
            {
                var selectedPrefab = (token.Item1 == string.Empty) ? _InputTokenPrefab : _stringTokenPrefab;
                var generated = Instantiate(selectedPrefab, this.transform);
                if (token.Item1 == string.Empty)
                {
                    var specialTokenCon = mustGetComponent<IInputTokenController>(generated);
                    FTBInputType currType = getFTBofGiven(token.Item2);
                    specialTokenCon.SetUpToken(token.Item2, currType);
                }
                else
                {
                    //Set displayed text and auto adjust text frame's size
                    var TMP = mustGetComponent<TextMeshProUGUI>(generated);
                    var rect = mustGetComponent<RectTransform>(generated);
                    //Assign display text;
                    TMP.text = token.Item1;
                    //Auto adjust text rect;
                    TMP.autoSizeTextContainer = true;
                    rect.sizeDelta = new Vector2(TMP.preferredWidth, tokenHeight);
                }
            }
        }
        private FTBInputType getFTBofGiven(FillTheBlankToken theToken)
        {
            if (theToken.GetType().Equals(typeof(TypingFTBToken))) return theToken.isLong ? FTBInputType.LONG_TYPING : FTBInputType.TYPING;
            else return FTBInputType.CHOICE;
        }
    }
}

