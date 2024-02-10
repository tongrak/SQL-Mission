using Assets.Scripts.DataPersistence.Chapter;
using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.Helper;
using Assets.Scripts.ScriptableObjects;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.MainMenuLayer
{
    public class MainMenuButtonManager : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingFacade;
        [SerializeField] private ChapterStatusDetailsData _chapterStatusDetailsData;
        [SerializeField] private bool _goToPlacement;

        private FileSystemWatcher _fileWatcher;
        private string _configsFolderPath;
        private string _statusFile;
        private int _totalStatusFile;
        private int _totalDeletedStatusFile;
        private bool _deleteCompleted;
        private bool _createChapterStatusCompleted;
        private string _chapterConfigsFolderFullPath;
        private string _chapterStatusFileFullPath;

        private void _SetFields()
        {
            _configsFolderPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, "Configs");
            _statusFile = EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType;
            _totalStatusFile = 0;
            _totalDeletedStatusFile = 0;
            _deleteCompleted = false;
            _chapterConfigsFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.ChapterConfigRootFolder;
            _chapterStatusFileFullPath = _chapterConfigsFolderFullPath + "/" + EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType;
        }

        public void ContinueButtonClicked()
        {
            _chapterStatusDetailsData.ChapterStatusDetails = _LoadChapterStatusDetailsFromFile();
            ScenesManager.Instance.LoadSelectChapterScene();
        }

        public void NewGameButtonClicked()
        {
            _loadingFacade.SetActive(true);
            if (_fileWatcher != null)
            {
                _DeleteAllStatusFile();
                // Will change scene after delete all status file and create chapter status file.
            }
            else
            {
                _CreateChapterStatusFile();
            }
        }

        #region Delete status file
        private void _DeleteAllStatusFile()
        {
            string[] statusfileList = Directory.GetFiles(_configsFolderPath, _statusFile + "*", SearchOption.AllDirectories);
            _totalStatusFile = statusfileList.Length;

            foreach (string statusfile in statusfileList)
            {
                File.Delete(statusfile);
            }
        }

        private void _StatusFileDeleted(object sender, FileSystemEventArgs e)
        {
            _totalDeletedStatusFile++;
            Debug.LogWarning("Deleted file: " + e.FullPath);

            if (_totalDeletedStatusFile == _totalStatusFile)
            {
                _deleteCompleted = true;
            }
        }
        #endregion

        #region Create chapter status file
        private void _CreateChapterStatusFile()
        {
            // 1) Load chpater config from json to object
            ChapterIndex chapterIndex = _LoadChapterIndexFromFile();
            ChapterConfig[] chapterConfigs = _LoadChapterConfigsFromFile(chapterIndex);

            // 2) Create chapter status object & Save to file
            ChapterStatusDetails chapterStatusDetails = _WriteChapterStatusDetailFile(chapterConfigs, _chapterStatusFileFullPath);

            // 3) Inject status object to SO
            _chapterStatusDetailsData.ChapterStatusDetails = chapterStatusDetails;
        }

        private void _ChapterStatusFileCreated(object sender, FileSystemEventArgs e)
        {
            _createChapterStatusCompleted = true;
        }

        private ChapterIndex _LoadChapterIndexFromFile()
        {
            string chapterIndexData = File.ReadAllText(_chapterConfigsFolderFullPath + "/" + EnvironmentData.Instance.ChpaterFileIndexFileName + EnvironmentData.Instance.ConfigFileType);
            ChapterIndex chapterIndex = JsonUtility.FromJson<ChapterIndex>(chapterIndexData);
            return chapterIndex;
        }

        private ChapterConfig[] _LoadChapterConfigsFromFile(ChapterIndex chapterIndex)
        {
            ChapterConfig[] chapterConfigs = new ChapterConfig[chapterIndex.ChapterFileIndex.Length];
            for (int i = 0; i < chapterConfigs.Length; i++)
            {
                string chapterFileName = chapterIndex.ChapterFileIndex[i];
                string chapterConfigData = File.ReadAllText(_chapterConfigsFolderFullPath + "/" + chapterFileName + EnvironmentData.Instance.ConfigFileType);
                chapterConfigs[i] = JsonUtility.FromJson<ChapterConfig>(chapterConfigData);
            }

            return chapterConfigs;
        }

        private ChapterStatusDetails _WriteChapterStatusDetailFile(ChapterConfig[] chapterConfigs, string chapterStatusFileFullPath)
        {
            ChapterStatusDetails chapterStatusDetails = new ChapterStatusDetails(chapterConfigs.Length);

            // Create status detail for each chapter.
            for (int i = 0; i < chapterConfigs.Length; i++)
            {
                ChapterConfig chapterConfig = chapterConfigs[i];
                int chapterDependencyNum = chapterConfig.ChapterDependencies.Length;
                bool chapterUnlocked = chapterDependencyNum <= 0;

                // Create dependency status.
                ChapterDependenciesStatusDetail[] chapterDependenciesStatusDetails = null;
                if (!chapterUnlocked)
                {
                    chapterDependenciesStatusDetails = new ChapterDependenciesStatusDetail[chapterDependencyNum];
                    for (int j = 0; j < chapterDependencyNum; j++)
                    {
                        chapterDependenciesStatusDetails[j] = new ChapterDependenciesStatusDetail
                        {
                            ChapterID = chapterConfig.ChapterDependencies[j],
                            IsPass = false
                        };
                    }
                }

                // Create status detail
                chapterStatusDetails.ChapterStatusDetailList[i] = new ChapterStatusDetail
                {
                    ChatperID = chapterConfig.ChapterID,
                    IsUnlock = chapterUnlocked,
                    IsPass = false,
                    ChapterDependenciesStatusDetail = chapterDependenciesStatusDetails
                };
            }

            // Save to 'StatusDetail.txt'
            string data = JsonUtility.ToJson(chapterStatusDetails, true);

            File.WriteAllText(chapterStatusFileFullPath, data);

            return chapterStatusDetails;
        }
        #endregion

        private ChapterStatusDetails _LoadChapterStatusDetailsFromFile()
        {
            string chapterStatusData = File.ReadAllText(_chapterStatusFileFullPath);
            ChapterStatusDetails chapterStatusDetails = JsonUtility.FromJson<ChapterStatusDetails>(chapterStatusData);
            return chapterStatusDetails;
        }

        private void _InitiateFileWatcher()
        {
            _fileWatcher = new FileSystemWatcher(_configsFolderPath, _statusFile + "*");
            _fileWatcher.NotifyFilter = NotifyFilters.CreationTime
             | NotifyFilters.LastWrite
             | NotifyFilters.Size;

            _fileWatcher.Deleted += _StatusFileDeleted;
            _fileWatcher.Created += _ChapterStatusFileCreated;

            _fileWatcher.IncludeSubdirectories = true;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private void Awake()
        {
            _SetFields();

            if (File.Exists(Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.ChapterConfigRootFolder, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType)))
            {
                _InitiateFileWatcher();
            }
        }

        private void Update()
        {
            if (_deleteCompleted)
            {
                _deleteCompleted = false;
                _CreateChapterStatusFile();
            }
            if (_createChapterStatusCompleted)
            {
                _createChapterStatusCompleted = false;
                ScenesManager.Instance.LoadSelectChapterScene();
            }
        }

        private void OnDestroy()
        {
            if (_fileWatcher != null)
            {
                _fileWatcher.Deleted -= _StatusFileDeleted;
                _fileWatcher.Created -= _ChapterStatusFileCreated;
            }
        }
    }
}