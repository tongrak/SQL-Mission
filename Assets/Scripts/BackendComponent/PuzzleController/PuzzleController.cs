using Assets.Scripts.DataPersistence.BlankBlockComponent;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.PuzzleManager;
using Assets.Scripts.DataPersistence.SQLComponent;
using Mono.Data.Sqlite;
using System;
using System.Linq;

namespace Assets.Scripts.DataPersistence.PuzzleController
{
    public class PuzzleController : IPuzzleController
    {
        private string _dbConn;
        private ISQLService _sqlService;
        private string[][] _answerTableResult;
        private readonly BlankOption[] _blankOptions;
        private int _passedChapterID;
        private IPuzzleManager _puzzleManager;

        public string Brief { get; private set; }
        public Schema[] Schemas { get; private set; }
        public string[][] PlayerTableResult { get; private set; }
        public PuzzleType PuzzleType { get; private set; }
        public VisualType VisualType { get; private set; }
        public string PreSQL { get; private set; }

        public PuzzleController(string dbConn, string answerSQL, string brief, Schema[] schemas, ISQLService sqlService, VisualType imgType, BlankOption[] blankOptions, string preSQL, PuzzleType puzzleType, int passedChapterID, IPuzzleManager puzzleManager)
        {
            _dbConn = dbConn;
            Brief = brief;
            Schemas = schemas;
            _sqlService = sqlService;
            VisualType = imgType;
            _answerTableResult = _sqlService.GetTableResult(dbConn, answerSQL, imgType);
            _blankOptions = blankOptions;
            PreSQL = preSQL;
            PuzzleType = puzzleType;
            _passedChapterID = passedChapterID;
            _puzzleManager = puzzleManager;
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

            if(_passedChapterID > 0 && isCorrect)
            {
                // Send signal to unlock chapter
                _puzzleManager.ChapterPassed(_passedChapterID);
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

        public string[] GetBlankOptions(string optionTitle)
        {
            foreach(BlankOption blankOption in _blankOptions)
            {
                if(optionTitle == blankOption.OptionTitle)
                {
                    return blankOption.OptionContext;
                }
            }

            return null;
        }
    }
}
