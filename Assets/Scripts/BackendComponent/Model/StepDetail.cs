namespace Assets.Scripts.DataPersistence.StepController
{
    [System.Serializable]
    public class StepDetail
    {
        public string Dialog;
        public Step Step; // IStep will be "Enum".
        public int PassedChapterID;
        public ImgDetail ImgDetail;
        public PuzzleDetail PuzzleDetail;
    }
}
