using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.VisualFeedback
{
    public interface IDynamicVisualController
    {
        void InitItemObjects(string[] imagePaths);
        void ShowUpGivenItem(string[] imagePaths);
        void ShowDownAll();
        void DiscontinueItemObjects();
    }

    public class DynamicVisualFeedbackController : GameplayController, IDynamicVisualController
    {
        [Header("ImageObjects Config")]
        [SerializeField] private GameObject[] _ImageObjects;

        private string[] _fileNames = new string[10];

        public void InitItemObjects(string[] imagePaths)
        {
            string[] clonedImagePaths = (string[])imagePaths.Clone();
            Array.Resize(ref clonedImagePaths, 10);

            _fileNames = clonedImagePaths.Select(getFilename).ToArray();

            setItemObjects(clonedImagePaths);
        }

        public void ShowUpGivenItem(string[] imagePaths)
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (imagePaths.Contains(_fileNames[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    StartCoroutine(showUpGiven(itemCon));
                }
            }
        }

        public void ShowDownAll()
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(_fileNames[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    StartCoroutine(showDownGiven(itemCon));
                }
            }
        }

        public void DiscontinueItemObjects()
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(_fileNames[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    itemCon.Deactivate();
                }
            }
        }

        #region Aux functions

        private string getFilename(string path)
        {
            if (path == null) return string.Empty;
            string[] tokens = path.Split('/');
            return tokens[tokens.Length - 1];
        }

        private void setItemObjects(string[] imagePaths)
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(imagePaths[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    itemCon.Activate(imagePaths[i]);
                }
                else
                {
                    _ImageObjects[i].SetActive(false);
                }
            }
        }

        private IEnumerator showUpGiven(ItemController ic)
        {
            ic.ShowUp();
            yield return null;
        }

        private IEnumerator showDownGiven(ItemController ic)
        {
            ic.ShowDown();
            yield return null;
        }

        #endregion

        #region Unity Basic
        private void Start()
        {
            foreach (var item in _ImageObjects)
                item.SetActive(false);
        }
        #endregion
    }
}
