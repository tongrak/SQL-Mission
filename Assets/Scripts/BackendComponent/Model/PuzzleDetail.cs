namespace Assets.Scripts.BackendComponent.StepController
{
    [System.Serializable]
    public class PuzzleDetail
    {
        public string DB; //Path to Database file.
        public string[] Tables; // Group of used table.
        public string PreSQL; // Can be blanked SQL or For executing SQL only.
        public string AnswerSQL;
        public VisualType VisualType; // PuzzleType will be "Enum".
        public PuzzleType PuzzleType;
        public string[][] SpecialBlankOptions;
    }
}
