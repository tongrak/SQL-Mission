namespace Assets.Scripts.DataPersistence.MissionStatusDetail
{
    [System.Serializable]
    public class MissionUnlockDetail
    {
        public int MissionID;
        public bool IsUnlock;
        public bool IsPass;
        public MissionDependencyUnlockDetail[] MissionDependenciesUnlockDetail;
    }
}
