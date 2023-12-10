using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI.VisualFeedback
{
    public interface IItemsController
    {
        void Activate(string imagePath);
        void Deactivate();
        void ShowUp();
        void ShowDown();
    }

    public class ItemController : GameplayController, IItemsController
    {
        [Header("Item config")]
        [SerializeField] private GameObject _itemObject;
        [SerializeField] private GameObject _showUpHeightRef;
        [SerializeField] private float _showSpeed;
        private Image _itemImage => mustGetComponent<Image>(_itemObject);
        private Vector3 _itemPosition => _itemObject.transform.position;
        private float _showUpHeight => _showUpHeightRef.transform.position.y;

        private bool _isActivate = false;
        private bool _isShowUp = false;
        private Vector3 _originPoint;
        private Vector3 _currentDestination;

        public void Activate(string imagePath)
        {
            if (_isActivate)
            {
                Debug.LogWarning("ItemController is already activated");
                return;
            }
            //Set origin point and initilize the currentDestination
            _currentDestination = _originPoint = this.transform.position;
            //Set display image
            _itemImage.sprite = Resources.Load<Sprite>(imagePath);

            _isActivate = true;
        }

        public void Deactivate()
        {
            ShowDown();
            _isActivate = false;
        }

        public void ShowUp()
        {
            //TODO: Utilize functions pipeline.
            if (!_isActivate )
            {
                Debug.LogWarning("ItemController is not activated");
                return;
            }
            if (!_isShowUp)
            {
                _currentDestination = new Vector3(_originPoint.x, _showUpHeight, _originPoint.z);
                _isShowUp = true;
            }
        }

        public void ShowDown()
        {
            if (!_isActivate)
            {
                Debug.LogWarning("ItemController is not activated");
                return;
            }
            if (_isShowUp)
            {
                _currentDestination = _originPoint;
                _isShowUp = false;
            }
        }

        #region UnityBasic
        private void Update()
        {
            if (_itemPosition != _currentDestination && _isActivate) 
                _itemObject.transform.position = Vector3.Lerp(_itemPosition, _currentDestination, _showSpeed * Time.deltaTime);
        }
        #endregion
    }
}
