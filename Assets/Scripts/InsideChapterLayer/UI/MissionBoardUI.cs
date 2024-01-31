using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.InsideChapterLayer.UI
{
    public class MissionBoardUI : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingFacade;
        private bool _fileCreated = false;

        private FileSystemWatcher _fileWatcher;
        public void Initiate(FileSystemWatcher watcher)
        {
            if (watcher != null)
            {
                _fileWatcher = watcher;
                _fileWatcher.Created += _StatusFileWrited;
            }
            else
            {
                _HideLoadingFacade();
            }
        }

        private void _StatusFileWrited(object sender, FileSystemEventArgs e)
        {
            _fileCreated = true;
        }

        private void _HideLoadingFacade()
        {
            _loadingFacade.SetActive(false);
        }

        #region Object disable or destroy
        private void OnDestroy()
        {
            _RemoveFuncFromWathcer();
        }

        private void OnDisable()
        {
            _RemoveFuncFromWathcer();
        }

        private void _RemoveFuncFromWathcer()
        {
            if (_fileWatcher != null)
            {
                _fileWatcher.Created -= _StatusFileWrited;
            }
        }
        #endregion

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if( _fileCreated)
            {
                _HideLoadingFacade();
                _fileCreated = false;
            }
        }
    }
}