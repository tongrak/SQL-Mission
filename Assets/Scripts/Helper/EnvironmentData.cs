﻿namespace Assets.Scripts.Helper
{
    public class EnvironmentData
    {
        public readonly string MissionConfigRootFolder = "MissionConfigs";
        public readonly string DatabaseRootFolder = "Database";
        public readonly string PuzzleImagesRootFolder = "PuzzleImages";

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