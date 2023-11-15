﻿using Assets.Scripts.PuzzleComponent;

namespace Assets.Scripts.MissionGenComponent.JSON_Class
{
    [System.Serializable]
    public class PuzzleDetail
    {
        public string DB; //Path to Database file.
        public string[] Tables; // Group of used table.
        public string AnswerSQL;
        public string ImgFolder; // Relative path for object's image
        public PuzzleType PuzzleType; // PuzzleType will be "Enum".
    }
}
