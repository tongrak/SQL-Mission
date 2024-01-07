namespace Assets.Scripts.BackendComponent.Model
{
    public class MissionConfig
    {
        public string MissionName;
        public string MissionDescription;
        public StepDetail[] MissionDetail;
        public MissionType MissionType;
        public string[] MissionDependencies;
    }
}
