using Assets.Scripts.PuzzleComponent.StepComponent;

namespace Assets.Scripts.MissionGenComponent.JSON_Class
{
    [System.Serializable]
    public class StepDetail
    {
        public string Dialog;
        public Step Step; // IStep will be "Enum".
        public PuzzleDetail Detail;
    }
}
