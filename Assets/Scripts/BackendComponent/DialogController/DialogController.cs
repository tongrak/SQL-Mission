using UnityEngine;

namespace Assets.Scripts.BackendComponent.DialogController
{
    public class DialogController : MonoBehaviour, IDialogController
    {
        private string[] _allDialog;

        /// <summary>
        /// Insert all Dialog for a mission.
        /// </summary>
        /// <param name="allDialog">Group of dialog.</param>
        public void SetAllDialog(string[] allDialog)
        {
            _allDialog = allDialog;
        }

        /// <summary>
        /// Get dialog from given index.
        /// </summary>
        public string GetDialog(int index)
        {
            return _allDialog[index];
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