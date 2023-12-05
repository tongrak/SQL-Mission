namespace Assets.Scripts.PuzzleComponent
{
    public interface IPuzzleController
    {
        string Brief { get; }
        Schema[] Schemas { get; }
        string[][] PlayerTableResult { get; }
        bool IsPass { get; }
        ExecuteResult GetExecuteResult(string sql);
        bool GetPuzzleResult();
        string[] GetTemplateBlank(string templateType, string table);
        string[] GetSpecialBlank(int index);
    }
}
