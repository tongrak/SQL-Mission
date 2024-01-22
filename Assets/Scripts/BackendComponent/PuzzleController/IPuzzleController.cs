﻿using Assets.Scripts.DataPersistence.MissionStatusDetail;

namespace Assets.Scripts.DataPersistence.PuzzleController
{
    public interface IPuzzleController
    {
        string Brief { get; }
        Schema[] Schemas { get; }
        string[][] PlayerTableResult { get; }
        PuzzleType PuzzleType { get; }
        VisualType VisualType { get; }
        string PreSQL { get; }
        ExecuteResult GetExecuteResult(string sql);
        bool GetPuzzleResult();
        string[] GetBlankOptions(string optionTitle);
    }
}
