using Assets.Scripts.BackendComponent.Model;

namespace Assets.Scripts.BackendComponent.PuzzleController
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
    }
}
