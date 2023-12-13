using Assets.Scripts.BackendComponent.StepComponent;

namespace Assets.Scripts.MissionGenComponent.Model
{
    [System.Serializable]
    public class StepDetail
    {
        public string Dialog;
        public Step Step; // IStep will be "Enum".
        public ImgDetail ImgDetail;
        public PuzzleDetail PuzzleDetail;
    }
}
