namespace Assets.Scripts.DataPersistence.ImageController
{
    public interface IImageController
    {
        void SetImagesList(string[][] imagesList);
        string[] GetImages(int stepIndex);
    }
}
