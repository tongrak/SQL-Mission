namespace Assets.Scripts.DataPersistence.MissionStatusDetail
{
    [System.Serializable]
    public class MissionStatusDetails
    {
        public MissionStatusDetail[] MissionStatusDetailList;

        public MissionStatusDetails(int length)
        {
            MissionStatusDetailList = new MissionStatusDetail[length];
        }
    }
}
