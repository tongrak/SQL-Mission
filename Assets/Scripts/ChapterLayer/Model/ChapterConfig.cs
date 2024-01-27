namespace Assets.Scripts.ChapterLayer.Model
{
    [System.Serializable]
    public class ChapterConfig
    {
        public int ChapterID;
        public string ChapterTitle;
        public string MissionConfigFolder;
        public string[] MissionFiles;
        public int[] ChapterDependencies;
    }
}
