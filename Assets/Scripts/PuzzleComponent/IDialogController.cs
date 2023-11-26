using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.PuzzleComponent
{
    public interface IDialogController
    {
        /// <summary>
        /// Insert all Dialog for a mission.
        /// </summary>
        /// <param name="allDialog">Group of dialog.</param>
        void SetAllDialog(string[] allDialog);

        /// <summary>
        /// Get dialog from given index.
        /// </summary>
        string GetDialog(int index);
    }
}
