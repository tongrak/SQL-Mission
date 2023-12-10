using Assets.Scripts.BackendComponent;

namespace Assets.Scripts.MissionGenComponent.Model
{
    [System.Serializable]
    public class PuzzleDetail
    {
        public string DB; //Path to Database file.
        public string[] Tables; // Group of used table.
        public string PreSQL; // Can be blanked SQL or For executing SQL only.
        public string AnswerSQL;
        public ImgType ImgType; // PuzzleType will be "Enum".
        public PuzzleType PuzzleType;
        public string[][] SpecialBlankOptions;
    }
}
