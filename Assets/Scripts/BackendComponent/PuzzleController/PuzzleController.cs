﻿using Assets.Scripts.BackendComponent.Model;
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

        public PuzzleResult GetPuzzleResult()
        {
            PuzzleResult puzzleResult = IsEqualQueryResult(_answerTableResult, PlayerTableResult);

            if(_passedChapterID > 0 && puzzleResult.IsCorrect)
            {
                // Send signal to unlock chapter
                _puzzleManager.ChapterPassed(_passedChapterID);
            }

            return puzzleResult;
        }

        private PuzzleResult IsEqualQueryResult(string[][] answerResult, string[][] playerResult)
        {
            string reason = String.Empty;

            if (playerResult == null)
            {
                reason = "The query does not produce a result.";
                return new PuzzleResult(false, reason);
            }
            else
            {
                if (answerResult.Length != playerResult.Length)
                {
                    // Column is not equal
                    if (playerResult.Length > answerResult.Length)
                    {
                        // Return result that tell player's column is more than answer
                        reason = "The query's number of column is more than expected.";
                    }
                    else
                    {
                        // Return result that tell player's column is less than answer
                        reason = "The query's number of column is more than expected.";
                    }
                    return new PuzzleResult(false, reason);
                }
                // Check each column
                else
                {
                    for (int i = 0; i < answerResult.Length; i++)
                    {
                        string[] currPlayerColumn = playerResult[i];
                        if (answerResult[i].Length != currPlayerColumn.Length)
                        {
                            if (answerResult[i].Length < currPlayerColumn.Length)
                            {
                                // Return result that tell player record is more than answer
                                reason = "The query's number of row is more than expected.";
                            }
                            else
                            {
                                // Return result that tell player record is less than answer
                                reason = "The query's number of row is less than expected.";
                            }
                            return new PuzzleResult(false, reason);
                        }
                        else
                        {
                            // 1) หา column ของ answer ที่ตรงกับ column ของ player ที่กำลังดูอยู่
                            int associateColumnIndex = Array.FindIndex(answerResult, (x => x[0] == currPlayerColumn[0]));

                            // 2) วนลูป check ทีละ tuple เพื่อดูว่าผลลัพธ์ที่ query ณ column ที่กำลังดูอยู่นั้นมานั้นถูกต้องหรือไม่
                            if (associateColumnIndex == -1)
                            {
                                reason = $"Column \"{currPlayerColumn[0]}\" is not in the expected result.";
                                return new PuzzleResult(false, reason);
                            }
                            else
                            {
                                // Check each tuple
                                for (int j = 0; j < answerResult[associateColumnIndex].Length; j++)
                                {
                                    if (!answerResult[associateColumnIndex][j].Equals(currPlayerColumn[j]))
                                    {
                                        reason = $"Incorrect query.";
                                        return new PuzzleResult(false, reason);
                                    }
                                }
                            }
                        }
                    }
                    return new PuzzleResult(true, reason);
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
