namespace Assets.Scripts.BackendComponent.PuzzleController
{
    public interface IPuzzleController
    {
        string Brief { get; }
        Schema[] Schemas { get; }
        string[][] PlayerTableResult { get; }
        PuzzleType PuzzleType { get; }
        VisualType VisualType { get; }
        ExecuteResult GetExecuteResult(string sql);
        bool GetPuzzleResult();
        string[] GetTemplateBlank(string templateType, string table);
        string[] GetSpecialBlank(int index);
    }
}
