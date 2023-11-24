﻿using Assets.Scripts.PuzzleComponent;

namespace Assets.Scripts.MissionGenComponent.JSON_Class
{
    [System.Serializable]
    public class PuzzleDetail
    {
        public string DB; //Path to Database file.
        public string[] Tables; // Group of used table.
        public string AnswerSQL;
        public ImgType ImgType; // PuzzleType will be "Enum".
        public PuzzleType PuzzleType;
        public string[][] SpecialBlankOptions;
    }
}
