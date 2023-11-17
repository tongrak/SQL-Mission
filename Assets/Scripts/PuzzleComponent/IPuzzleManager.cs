using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent
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
    }
}
