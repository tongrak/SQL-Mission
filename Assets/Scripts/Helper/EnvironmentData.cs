namespace Assets.Scripts.Helper
{
    public class EnvironmentData
    {
        public readonly string MissionConfigRootFolder = "MissionConfigs";
        public readonly string DatabaseRootFolder = "Database";
        public readonly string PuzzleImagesRootFolder = "PuzzleImages";
        public readonly string MissionStatusFileName = "UnlockDetail";
        public readonly string MissionStatusDetailFileType = ".txt"; // Must be ".txt" of ".json"
        public readonly string ImageColumn = "Image";

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