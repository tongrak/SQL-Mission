using Assets.Scripts.BackendComponent.PuzzleController;
using Assets.Scripts.BackendComponent.SaveManager;

namespace Assets.Scripts.BackendComponent.PuzzleManager
{
    public interface IPuzzleManager
    {
        /// <summary>
        /// Use for construct puzzle manager.
        /// </summary>
        /// <param name="allPC">Group of puzzle controller.</param>
        void Construct(IPuzzleController[] allPC);

        /// <summary>
        /// Get Puzzle Controller from given index.
        /// </summary>
        IPuzzleController GetPC(int index);

        /// <summary>
        /// Update mission status if final puzzle passed.
        /// </summary>
        void AllPuzzlePassed();
    }
}
