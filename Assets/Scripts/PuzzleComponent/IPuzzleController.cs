namespace Assets.Scripts.PuzzleComponent
{
    public interface IPuzzleController
    {
        string Brief { get; }
        Schema[] Schema { get; }
        PuzzleResult GetResult(string sql);
    }
}
