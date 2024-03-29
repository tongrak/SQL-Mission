namespace Assets.Scripts.DataPersistence.MissionStatusDetail
{
    [System.Serializable]
    public class MissionStatusDetail
    {
        public int MissionID;
        public bool IsUnlock;
        public bool IsPass;
        public MissionDependencyStatusDetail[] MissionDependenciesStatusDetail;
    }
}
