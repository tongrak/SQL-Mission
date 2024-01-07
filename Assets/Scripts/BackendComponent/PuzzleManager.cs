﻿using UnityEngine;

namespace Assets.Scripts.BackendComponent
{
    public class PuzzleManager : MonoBehaviour, IPuzzleManager
    {
        private IPuzzleController[] _allPC;

        /// <summary>
        /// Insert all PC for a mission.
        /// </summary>
        /// <param name="allPC">Group of puzzle controller.</param>
        public void SetAllPC(IPuzzleController[] allPC)
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

        public void PuzzlePassed(bool isLastPuzzle)
        {

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