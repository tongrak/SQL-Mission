using Assets.Scripts.BackendComponent.BlankBlockComponent;
using Assets.Scripts.BackendComponent.SQLComponent;
using Mono.Data.Sqlite;
using System;
using System.Linq;

namespace Assets.Scripts.BackendComponent
{
    public class PuzzleController : IPuzzleController
    {
        private string _dbConn;
        private ISQLService _sqlService;
        private string[][] _answerTableResult;
        private IFixedTemplateService _fixedTemplateService;
        private IUpToConfigTemplateService _upToConfigTemplateService;
        private readonly string[][] _specialBlanks;

        public string Brief { get; private set; }
        public Schema[] Schemas { get; private set; }
        public string[][] PlayerTableResult { get; private set; }
        public bool IsPass { get; private set; }
        public PuzzleType PuzzleType { get; private set; }
        public VisualType VisualType { get; private set; }
        public bool IsLastPuzzle { get; private set; }

        public PuzzleController(string dbConn, string answerSQL, string brief, Schema[] schemas, ISQLService sqlService, VisualType imgType, IFixedTemplateService fixedTemplateService, IUpToConfigTemplateService upToConfigTemplateService, string[][] specialBlanks, bool isLastPuzzle)
        {
            _dbConn = dbConn;
            Brief = brief;
            Schemas = schemas;
            _sqlService = sqlService;
            VisualType = imgType;
            _answerTableResult = _sqlService.GetTableResult(dbConn, answerSQL, imgType);
            _fixedTemplateService = fixedTemplateService;
            _upToConfigTemplateService = upToConfigTemplateService;
            _specialBlanks = specialBlanks;
            IsLastPuzzle = isLastPuzzle;
        }

        public ExecuteResult GetExecuteResult(string playerSQL)
        {
            try
            {
                PlayerTableResult = _sqlService.GetTableResult(_dbConn, playerSQL, VisualType);
                return new ExecuteResult(PlayerTableResult);
            }
            catch (Exception e)
            { 
                return new ExecuteResult(e.Message);
            }
        }

        public bool GetPuzzleResult()
        {
            bool isCorrect = IsEqualQueryResult(_answerTableResult, PlayerTableResult);
            if(!IsPass && isCorrect)
            {
                IsPass = true;
            }

            return isCorrect;
        }

        private bool IsEqualQueryResult(string[][] query1, string[][] query2)
        {
            if (query1 == null || query2 == null)
            {
                return false;
            }
            else
            {
                if (query1.Length != query2.Length)
                {
                    return false;
                }
                // Check each attribute
                else
                {
                    for (int i = 0; i < query1.Length; i++)
                    {
                        // Check number of record from each attribute
                        if (!query1[i].SequenceEqual(query2[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public string[] GetTemplateBlank(string templateType, string table)
        {
            switch (templateType)
            {
                case "OperatorsSymbol":
                    return _fixedTemplateService.OperatorsSymbol;
                case "OperatorsWord":
                    return _fixedTemplateService.OperatorsWord;
                case "Function":
                    return _fixedTemplateService.Function;
                case "Command":
                    return _fixedTemplateService.Command;
                case "Tables":
                    return _upToConfigTemplateService.GetTablesTemplate(_dbConn);
                case "Schema":
                    return _upToConfigTemplateService.GetSchemaTemplate(_dbConn, table);
                case "Attributes":
                    return _upToConfigTemplateService.GetAttributesTemplate(_dbConn, table);
                default:
                    return null;
            }
        }

        public string[] GetSpecialBlank(int index)
        {
            return _specialBlanks[index];
        }
    }
}
