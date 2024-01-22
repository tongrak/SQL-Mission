namespace Assets.Scripts.DataPersistence.MissionStatusDetail
{
    [System.Serializable]
    public class MissionUnlockDetails
    {
        public MissionUnlockDetail[] MissionUnlockDetailList;

        public MissionUnlockDetails(int length)
        {
            MissionUnlockDetailList = new MissionUnlockDetail[length];
        }
    }
}
