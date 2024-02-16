namespace Assets.Scripts.DataPersistence.Chapter
{
    [System.Serializable]
    public class ChapterConfig
    {
        public int ChapterID;
        public string ChapterTitle;
        public string MissionConfigFolder;
        public string[] MissionFilesIndex;
        public int[] ChapterDependencies;
    }
}
