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

        public void SetUpLineToken((string, FillTheBlankToken)[] tokens)
        {
            foreach (var token in tokens)
            {
                var selectedPrefab = (token.Item1 == string.Empty) ? _InputTokenPrefab : _stringTokenPrefab;
                var generated = Instantiate(selectedPrefab, this.transform);
                if (token.Item1 == string.Empty)
                {
                    var specialTokenCon = mustGetComponent<IInputTokenController>(generated);
                    //TODO: check input type
                    FTBInputType currType = token.Item2.GetType().Equals(typeof(TypingFTBToken)) ? FTBInputType.TYPING : FTBInputType.CHOICE;
                    specialTokenCon.SetUpToken(token.Item2, currType);
                }
                else
                {
                    //Set displayed text and auto adjust text frame's size
                    var TMP = mustGetComponent<TextMeshProUGUI>(generated);
                    TMP.text = token.Item1;
                    TMP.autoSizeTextContainer = true;
                }
            }
        }
    }
}

