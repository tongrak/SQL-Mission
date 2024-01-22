namespace Assets.Scripts.DataPersistence.DialogController
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
