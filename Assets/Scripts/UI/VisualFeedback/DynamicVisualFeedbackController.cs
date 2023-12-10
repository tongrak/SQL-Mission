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

        private string[] _currentImagePaths = new string[10];

        public void InitItemObjects(string[] imagePaths)
        {
            string[] copyTheGiven = (string[])imagePaths.Clone();
            Array.Resize(ref copyTheGiven, _ImageObjects.Length);

            _currentImagePaths = copyTheGiven;

            setItemObjects(_currentImagePaths);
        }

        public void testInitFunc()
        {
            string[] simpleImagePaths = { "Sprites/Flower/Pic1", "Sprites/Flower/Pic2", "Sprites/Flower/Pic3" };
            InitItemObjects(simpleImagePaths);
        }

        public void ShowUpGivenItem(string[] imagePaths)
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (imagePaths.Contains(_currentImagePaths[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    StartCoroutine(showUpGiven(itemCon));
                }
            }
        }

        public void textShowUpFunc()
        {
            string[] simpleImagePaths = { "Sprites/Flower/Pic1", "Sprites/Flower/Pic3" };
            ShowUpGivenItem(simpleImagePaths) ;

        }

        public void ShowDownAll()
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(_currentImagePaths[i]))
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
                if (!string.IsNullOrEmpty(_currentImagePaths[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    itemCon.Deactivate();
                    _ImageObjects[i].SetActive(false);
                }
            }
        }

        #region Aux functions

        private void setItemObjects(string[] imagePaths)
        {
            for (int i = 0; i < _ImageObjects.Length; i++)
            {
                if (!string.IsNullOrEmpty(imagePaths[i]))
                {
                    var itemCon = mustGetComponent<ItemController>(_ImageObjects[i]);
                    _ImageObjects[i].SetActive(true);

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
