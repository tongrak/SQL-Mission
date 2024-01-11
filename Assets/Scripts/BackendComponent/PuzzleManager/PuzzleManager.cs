using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.BackendComponent.PuzzleController;
using Assets.Scripts.BackendComponent.SaveManager;
using Assets.Scripts.BackendComponent.StepController;
using UnityEngine;

namespace Assets.Scripts.BackendComponent.PuzzleManager
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

        public void AllPuzzlePassed()
        {
            _missionController.AllPuzzlePassed();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}