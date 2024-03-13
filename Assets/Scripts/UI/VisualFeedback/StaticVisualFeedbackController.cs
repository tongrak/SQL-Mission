using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.VisualFeedback
{
    public interface IStaticVisualController: IGeneralVisualController
    {
        void InitItemObjects(string[] imagePaths);
    }
    public class StaticVisualFeedbackController : GameplayController, IStaticVisualController
    {
        [Header("ImageObjects Config")]
        [SerializeField] private GameObject[] _ImageObjects;
        [SerializeField] private readonly int _maxImageNumber = 3;
        public void DiscontinueVisualItemObjects()
        {
            foreach (var item in _ImageObjects) item.SetActive(false);
        }
        public void InitItemObjects(string[] imagePaths)
        {
            if (imagePaths.Length > 3) Debug.LogWarning("Static visual can only display " + _maxImageNumber + " images");
            string[] clonedImagePaths = (string[])imagePaths.Clone();
            Array.Resize(ref clonedImagePaths, _maxImageNumber);
            //Set given Paths to display
            setItemObjects(clonedImagePaths);
        }
        private void setItemObjects(string[] imagePaths)
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(imagePaths[i]))
                {
                    //Set image, size&ratio, and set to fit;
                    var imageCon = mustGetComponent<Image>(_ImageObjects[i]);
                    var aspectRatioCon = mustGetComponent<AspectRatioFitter>(_ImageObjects[i]);
                    var imageSprite = Resources.Load<Sprite>(imagePaths[i]);
                    imageCon.sprite = imageSprite;
                    if (imageCon.sprite == null)
                    {
                        Debug.LogWarning("Can not load image with path:" + imagePaths[i]);
                        break;
                    }
                    //imageCon.SetNativeSize();
                    float imageRatio = imageSprite.rect.width / imageSprite.rect.height;
                    aspectRatioCon.aspectRatio = imageRatio;
                    aspectRatioCon.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                    //Show image;
                    _ImageObjects[i].SetActive(true);
                }
                else
                {
                    _ImageObjects[i].SetActive(false);
                }
            }
        }
        #region Unity Basic
        private void Start()
        {
            foreach (var item in _ImageObjects)
                item.SetActive(false);

        }
        #endregion
    }

}


