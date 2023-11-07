namespace Assets.Scripts.PuzzleComponent
{
    public class PuzzleResult
    {
        public bool IsCorrect { get; private set; }
        public string[][] TableResult { get; private set; }
        public bool IsError { get; private set; }
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// For create error result.
        /// </summary>
        /// <param name="errMsg">Error message.</param>
        public PuzzleResult(string errMsg)
        {
            IsCorrect = false;
            IsError = true;
            ErrorMessage = errMsg;
        }

        /// <summary>
        /// For create valid result.
        /// </summary>
        /// <param name="tableResult">Record result from query execute.</param>
        /// <param name="isCorrect">Correctness from query execute.</param>
        public PuzzleResult(string[][] tableResult, bool isCorrect)
        {
            IsCorrect = isCorrect;
            IsError = false;
            TableResult = tableResult;
        }
    }
}
