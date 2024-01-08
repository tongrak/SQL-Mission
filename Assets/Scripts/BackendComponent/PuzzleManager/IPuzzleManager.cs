using Assets.Scripts.BackendComponent.PuzzleController;

namespace Assets.Scripts.BackendComponent.PuzzleManager
{
    public interface IPuzzleManager
    {
        /// <summary>
        /// Insert all PC for a mission.
        /// </summary>
        /// <param name="allPC">Group of puzzle controller.</param>
        void SetAllPC(IPuzzleController[] allPC);

        /// <summary>
        /// Get Puzzle Controller from given index.
        /// </summary>
        IPuzzleController GetPC(int index);

        /// <summary>
        /// Update mission status if final puzzle passed.
        /// </summary>
        void PuzzlePassed(bool isLastPuzzle);
    }
}
