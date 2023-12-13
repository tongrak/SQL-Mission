﻿namespace Assets.Scripts.BackendComponent
{
    public interface IPuzzleController
    {
        string Brief { get; }
        Schema[] Schemas { get; }
        string[][] PlayerTableResult { get; }
        bool IsPass { get; }
        PuzzleType PuzzleType { get; }
        ImgType ImgType { get; }
        ExecuteResult GetExecuteResult(string sql);
        bool GetPuzzleResult();
        string[] GetTemplateBlank(string templateType, string table);
        string[] GetSpecialBlank(int index);
    }
}
