using UnityEngine;

namespace Assets.Scripts.Helper
{
    public class EnvironmentData
    {
        public readonly string MissionConfigRootFolder = "Configs/MissionConfigs";
        public readonly string DatabaseRootFolder = "Database";
        public readonly string PuzzleImagesRootFolder = "PuzzleImages";
        public readonly string ConfigFileType = ".json"; // Must be ".txt" or ".json"
        public readonly string ImageColumn = "Image";
        public readonly string ChapterConfigRootFolder = "Configs/ChapterConfigs";
        public readonly string ChpaterFileIndexFileName = "ChapterFileIndex";
        public readonly string StatusFileName = "StatusDetail";
        public readonly string ResourcesFolder = "Resources";
        public readonly string PlacementConfigRootFolder = "Configs/PlacementConfig";
        public readonly string PlacementFileName = "Placement";

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