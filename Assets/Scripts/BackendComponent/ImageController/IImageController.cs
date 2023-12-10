namespace Assets.Scripts.BackendComponent.ImageController
{
    public interface IImageController
    {
        void SetImagesList(string[][] imagesList);
        string[] GetImages(int stepIndex);
    }
}
