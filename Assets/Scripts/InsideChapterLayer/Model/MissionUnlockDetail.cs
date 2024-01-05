namespace Assets.Scripts.InsideChapterLayer.Model
{
    [System.Serializable]
    public class MissionUnlockDetail
    {
        public string MissionName;
        public bool IsUnlock;
        public bool IsPass;
        public MissionDependencyUnlockDetail[] MissionDependenciesUnlockDetail;
    }
}
