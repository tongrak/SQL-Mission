namespace Assets.Scripts.DataPersistence.ChapterStatusDetail
{
    [System.Serializable]
    public class ChapterStatusDetails
    {
        public ChapterStatusDetail[] ChapterStatusDetailList;

        public ChapterStatusDetails(int length)
        {
            ChapterStatusDetailList = new ChapterStatusDetail[length];
        }
    }
}
