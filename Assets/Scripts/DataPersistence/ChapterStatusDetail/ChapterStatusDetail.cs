namespace Assets.Scripts.DataPersistence.ChapterStatusDetail
{
    [System.Serializable]
    public class ChapterStatusDetail
    {
        public int ChatperID;
        public bool IsUnlock;
        public bool IsPass;
        public ChapterDependenciesStatusDetail[] ChapterDependenciesStatusDetail;
    }
}
