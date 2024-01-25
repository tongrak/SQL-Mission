namespace Assets.Scripts.DataPersistence.StepController
{
    public class MissionConfig
    {
        public int MissionID;
        public string MissionTitle;
        public string MissionDescription;
        public StepDetail[] MissionDetail;
        public MissionType MissionType;
        public int[] MissionDependencies;
        public int[] MissionDependTos;
    }
}
