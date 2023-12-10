using Assets.Scripts.BackendComponent.StepComponent;

namespace Assets.Scripts.MissionGenComponent.Model
{
    [System.Serializable]
    public class StepDetail
    {
        public string Dialog;
        public Step Step; // IStep will be "Enum".
        public string ImgFolder;
        public string[] ImgList;
        public PuzzleDetail Detail;
    }
}
