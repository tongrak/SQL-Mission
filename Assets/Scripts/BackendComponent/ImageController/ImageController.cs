using UnityEngine;

namespace Assets.Scripts.BackendComponent.ImageController
{
    public class ImageController : MonoBehaviour, IImageController
    {
        private string[][] _imagesList;

        public string[] GetImages(int stepIndex)
        {
            return _imagesList[stepIndex];
        }

        public void SetImagesList(string[][] imagesList)
        {
            _imagesList = imagesList;
        }
    }
}
