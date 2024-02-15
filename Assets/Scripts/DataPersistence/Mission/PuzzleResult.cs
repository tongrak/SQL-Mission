namespace Assets.Scripts.BackendComponent.Model
{
    public class PuzzleResult
    {
        public readonly bool IsCorrect;
        public readonly string Reason;

        public PuzzleResult(bool isCorrect, string reason)
        {
            IsCorrect = isCorrect;
            Reason = reason;
        }
    }
}
