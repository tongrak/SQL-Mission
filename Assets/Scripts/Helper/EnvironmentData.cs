using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class EnvironmentData
    {
        public readonly string MissionConfigRootFolder = "Configs/MissionConfigs";
        public readonly string DatabaseRootFolder = "Database";
        public readonly string PuzzleImagesRootFolder = "PuzzleImages";
        public readonly string ConfigFileType = ".txt"; // Must be ".txt" of ".json"
        public readonly string ImageColumn = "Image";
        public readonly string ChapterConfigRootFolder = "Configs/ChapterConfigs";
        public readonly string ChpaterFileIndexFileName = "ChapterFileIndex";
        public readonly string StatusFileName = "StatusDetail";
        public readonly string ResourcesFolder = "Resources";

        private static readonly EnvironmentData instance = new EnvironmentData();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static EnvironmentData()
        {
        }

        private EnvironmentData()
        {
        }

        public static EnvironmentData Instance
        {
            get
            {
                return instance;
            }
        }
    }
}