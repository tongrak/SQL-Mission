using Assets.Scripts.DataPersistence.PuzzleController;
using Assets.Scripts.DataPersistence.SaveManager;

namespace Assets.Scripts.DataPersistence.PuzzleManager
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

        void ChapterPassed(int passedChapterID);
    }
}
