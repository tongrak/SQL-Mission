using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using Gameplay.UI.Construction.FTB;

namespace Gameplay.UI.Construction
{
    /// <summary>
    /// A interface to clickable token (FTB token). All FTB must define ClickAction method for Unity button component usage.
    /// </summary>
    public abstract class FillTheBlankToken
    {
        protected string _optionName;
        protected string _selectString;
        public bool isLong = false;

        public string selectToken { get => _selectString; }
        public void SetSelectedString(string selected) => this._selectString = selected;
        public abstract string[] GetContextOptions();
    }
    class ChoiceFTBToken : FillTheBlankToken
    {
        private Func<string, string[]> _getOptionOfKey;
        public ChoiceFTBToken(Func<string, string[]> getOptionFunction, string tokenType)
        {
            this._optionName = tokenType;
            this._getOptionOfKey = getOptionFunction;
        }
        public override string[] GetContextOptions() => _getOptionOfKey(_optionName);
    }
    class TypingFTBToken : FillTheBlankToken
    {
        public override string[] GetContextOptions() => null;
        public TypingFTBToken(bool isLong) => base.isLong = isLong;
    }
    
    public class FillTheBlankQueryController : GameplayController, IFillTheBlankQuery
    {
        [Header("Tokenize configuration")]
        [SerializeField] private string _newlineSplitPoint = "\n";
        [SerializeField] private string _FTBTokenReg = @"<<(.*?)>>";
        [SerializeField] private string _typeFTBTokenReg = @"Type";
        [SerializeField] private string _typeLongFTBTokenReg = @"TypeLong";
        //[SerializeField] private string _specialFTBTokenReg = @"[A-Za-z0-9]+:[A-Za-z0-9]+";

        [Header("UI component")]
        [SerializeField] private GameObject _lineTokenPrefab;

        private (string, FillTheBlankToken)[][] _currentTokenField = null;

        public string queryString
        {
            get
            {
                (string, FillTheBlankToken)[] oneLinedToken = _currentTokenField.Aggregate((arr, crr) => arr.Concat(crr).ToArray());
                string[] queriedTokens = oneLinedToken.Select(getQueryStringToken).ToArray();
                return string.Join(" ", queriedTokens);
            }
        }
        public void SetUpTokenField(Func<string, string[]> getOptionFunction, string tokens)
        {
            //remove pass token
            startConsole();
            //Split tokenfields to tokenlines
            string[] lineTokens = tokens.Split(new string[] { _newlineSplitPoint }, StringSplitOptions.None);
            //Split tokenlines to tokens then filter all
            string[][] tokensField = lineTokens.Select(splitAndFilter).ToArray();
            //Convert tokenField to FTB
            Func<string[], (string, FillTheBlankToken)[]> convertTokenLineToFTBline = (tokenLine) => tokenLine.Select(x => stringToFTBToken(getOptionFunction, x)).ToArray();
            this._currentTokenField = tokensField.Select(convertTokenLineToFTBline).ToArray();
            //Create the fill the blank token field from given string.
            foreach (var line in _currentTokenField)
            {
                GameObject createdLine = Instantiate(_lineTokenPrefab, this.transform);
                ILineTokenController lineCon = mustGetComponent<ILineTokenController>(createdLine);
                lineCon.SetUpLineToken(line);
            }
        }
        public void startConsole()
        {
            //remove past tokens
            foreach (Transform token in this.transform)
            {
                if (token.gameObject != null) Destroy(token.gameObject);
            }
        }

        #region Aux method
        private string getQueryStringToken((string, FillTheBlankToken) token) => (token.Item1 != string.Empty) ? token.Item1 : token.Item2.selectToken;
        #endregion

        #region Tokenizer
        private string[] splitAndFilter(string given) => given.Split(' ').ToArray()
            .Where(x => !(string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x))).ToArray();
        private (string, FillTheBlankToken) stringToFTBToken(Func<string, string[]> getOptionFunction, string token)
        {
            //If given string doesn't match as agreed, assume as normal string.
            if (!Regex.IsMatch(token, _FTBTokenReg)) return new(token, null);
            //Else attempted to generate Fill the blank
            //Get string in between <<>>
            string realToken = Regex.Match(token, _FTBTokenReg).Groups[1].Value;
            return new(string.Empty, FTBTokenGenetion(getOptionFunction, realToken));
        }
        private FillTheBlankToken FTBTokenGenetion(Func<string, string[]> getOptionFunction, string token)
        {
            if (Regex.IsMatch(token, _typeFTBTokenReg))
            {
                if (Regex.IsMatch(token, _typeLongFTBTokenReg)) return new TypingFTBToken(true);
                else return new TypingFTBToken(false);
            }
            else return new ChoiceFTBToken(getOptionFunction, token);
        }
        #endregion
    }
}   