using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.PuzzleController;
using Assets.Scripts.DataPersistence.SaveManager;
using Assets.Scripts.DataPersistence.StepController;
using UnityEngine;

namespace Assets.Scripts.DataPersistence.PuzzleManager
{
    public class PuzzleManager : MonoBehaviour, IPuzzleManager
    {
        [SerializeField] private MissionController _missionController;

        private IPuzzleController[] _allPC;

        /// <summary>
        /// Insert all PC for a mission.
        /// </summary>
        /// <param name="allPC">Group of puzzle controller.</param>
        public void Construct(IPuzzleController[] allPC)
        {
            _allPC = allPC;
        }

        /// <summary>
        /// Get Puzzle Controller from given index.
        /// </summary>
        public IPuzzleController GetPC(int index)
        {
            return _allPC[index];
        }
        
        public void ChapterPassed(int passedChapterID)
        {
            _missionController.ChapterPassed(passedChapterID);
        }
    }
}