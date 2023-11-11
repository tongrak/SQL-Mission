using Assets.Scripts.PuzzleComponent.SQLComponent;
using Mono.Data.Sqlite;
using System;
using System.Linq;

namespace Assets.Scripts.PuzzleComponent
{
    public class PuzzleController : IPuzzleController
    {
        private string _dbPath;
        private ISQLService _sqlService;
        private string[][] _answerTableResult;
        private readonly PuzzleType _puzzleType;

        public string Brief { get; private set; }
        public Schema[] Schema { get; private set; }
        public string[][] PlayerTableResult { get; private set; }
        public bool IsPass { get; private set; }

        public PuzzleController(string dbPath, string answerSQL, string brief, Schema[] schema, ISQLService sqlService, PuzzleType puzzleType)
        {
            _dbPath = dbPath;
            Brief = brief;
            Schema = schema;
            _sqlService = sqlService;
            _puzzleType = puzzleType;
            _answerTableResult = _sqlService.GetTableResult(dbPath, answerSQL, puzzleType);
        }

        public ExecuteResult GetExecuteResult(string playerSQL)
        {
            try
            {
                PlayerTableResult = _sqlService.GetTableResult(_dbPath, playerSQL, _puzzleType);
                return new ExecuteResult(PlayerTableResult);
            }
            catch (SqliteException e)
            { 
                return new ExecuteResult(e.Message);
            }
        }

        public bool GetPuzzleResult()
        {
            bool currIsPass = IsEqualQueryResult(_answerTableResult, PlayerTableResult);
            if(!IsPass && currIsPass)
            {
                IsPass = true;
            }

            return currIsPass;
        }

        private bool IsEqualQueryResult(string[][] query1, string[][] query2)
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
}
