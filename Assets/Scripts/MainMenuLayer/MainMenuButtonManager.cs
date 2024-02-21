using Assets.Scripts.DataPersistence.Chapter;
using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.StepController;
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
        [SerializeField] private SelectedChapterData _selectedChapterData;
        [SerializeField] private bool _goToPlacement;
        [SerializeField] private MissionData _missionSceneData;

        private string _chapterConfigsFolderFullPath;
        private string _chapterStatusFileFullPath;

        private void _SetFields()
        {
            _chapterConfigsFolderFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.ChapterConfigRootFolder);
            _chapterStatusFileFullPath = Path.Combine(_chapterConfigsFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType);
        }

        public void ContinueButtonClicked()
        {
            _chapterStatusDetailsData.ChapterStatusDetails = _LoadChapterStatusDetailsFromFile();
            ScenesManager.Instance.LoadSelectChapterScene();
        }

        public void NewGameButtonClicked()
        {
            _loadingFacade.SetActive(true);

            _CreateChapterStatusFile();
            if (_goToPlacement)
            {
                _GoToPlacementTest();
            }
            else
            {
                ScenesManager.Instance.LoadSelectChapterScene();
            }
        }

        //#region Delete status file
        //private void _DeleteAllStatusFile()
        //{
        //    string[] statusfileList = Directory.GetFiles(_configsFolderPath, _statusFile + "*", SearchOption.AllDirectories);
        //    _totalStatusFile = statusfileList.Length;

        //    foreach (string statusfile in statusfileList)
        //    {
        //        File.Delete(statusfile);
        //    }
        //}
        //#endregion

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

            // 4) Create or overwrite mission status detail
            _CreateAllMissionStatusFile(chapterConfigs);
        }

        private void _CreateAllMissionStatusFile(ChapterConfig[] chapterConfigs)
        {
            foreach(ChapterConfig chapterConfig in chapterConfigs)
            {
                string statusDetailFileFullPathWithFormat = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.MissionConfigRootFolder, chapterConfig.MissionConfigFolder, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType);

                // 1) Get every mission config for current chapter.
                MissionConfig[] missionConfigs = new MissionConfig[chapterConfig.MissionFilesIndex.Length];
                for(int i = 0; i < chapterConfig.MissionFilesIndex.Length; i++)
                {
                    string missionConfigFileName = chapterConfig.MissionFilesIndex[i];
                    string configFileFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.MissionConfigRootFolder, chapterConfig.MissionConfigFolder, missionConfigFileName + EnvironmentData.Instance.ConfigFileType);
                    string configData = File.ReadAllText(configFileFullPath);
                    missionConfigs[i] = JsonUtility.FromJson<MissionConfig>(configData);
                }

                // 2) Create mission status detail.
                _WriteMissionStatusDetails(missionConfigs, statusDetailFileFullPathWithFormat);
            }
        }

        private MissionUnlockDetails _WriteMissionStatusDetails(MissionConfig[] missionConfigs, string statusDetailFileFullPath)
        {
            MissionUnlockDetails missionStatusDetails = new MissionUnlockDetails(missionConfigs.Length);

            // Create unlock detail for each mission.
            for (int i = 0; i < missionConfigs.Length; i++)
            {
                MissionConfig missionConfig = missionConfigs[i];
                int missionDependencyNum = missionConfig.MissionDependencies.Length;
                bool isMissionUnlocked = missionDependencyNum <= 0;

                MissionDependencyUnlockDetail[] missionDependenciesUnlockDetail = null;
                if (!isMissionUnlocked)
                {
                    missionDependenciesUnlockDetail = new MissionDependencyUnlockDetail[missionDependencyNum];
                    for (int k = 0; k < missionDependencyNum; k++)
                    {
                        missionDependenciesUnlockDetail[k] = new MissionDependencyUnlockDetail
                        {
                            MissionID = missionConfig.MissionDependencies[k],
                            IsPass = false
                        };
                    }
                }

                // Create unlock detail
                missionStatusDetails.MissionUnlockDetailList[i] = new MissionUnlockDetail
                {
                    MissionID = missionConfig.MissionID,
                    IsUnlock = isMissionUnlocked,
                    IsPass = false,
                    MissionDependenciesUnlockDetail = missionDependenciesUnlockDetail
                };
            }

            // Save to 'UnlockDetail.txt'
            string data = JsonUtility.ToJson(missionStatusDetails, true);

            File.WriteAllText(statusDetailFileFullPath, data);

            return missionStatusDetails;
        }

        private ChapterIndex _LoadChapterIndexFromFile()
        {
            string chapterIndexData = File.ReadAllText(Path.Combine(_chapterConfigsFolderFullPath, EnvironmentData.Instance.ChpaterFileIndexFileName + EnvironmentData.Instance.ConfigFileType));
            ChapterIndex chapterIndex = JsonUtility.FromJson<ChapterIndex>(chapterIndexData);
            return chapterIndex;
        }

        private ChapterConfig[] _LoadChapterConfigsFromFile(ChapterIndex chapterIndex)
        {
            ChapterConfig[] chapterConfigs = new ChapterConfig[chapterIndex.ChapterFileIndex.Length];
            for (int i = 0; i < chapterConfigs.Length; i++)
            {
                string chapterFileName = chapterIndex.ChapterFileIndex[i];
                string chapterConfigData = File.ReadAllText(Path.Combine(_chapterConfigsFolderFullPath, chapterFileName + EnvironmentData.Instance.ConfigFileType));
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

        private void _GoToPlacementTest()
        {
            string placementConfigData = File.ReadAllText(Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.PlacementConfigRootFolder, EnvironmentData.Instance.PlacementFileName + EnvironmentData.Instance.ConfigFileType));
            MissionConfig placementConfig = JsonUtility.FromJson<MissionConfig>(placementConfigData);
            MissionConfig[] missionConfigs = new MissionConfig[1] { placementConfig };

            // Set mission data
            _missionSceneData.missionConfigs = missionConfigs;
            _missionSceneData.missionConfigIndex = 0;
            _missionSceneData.MissionConfigFolderFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.PlacementConfigRootFolder);

            // Set selected chapter data
            _selectedChapterData.ChapterFolderFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.ChapterConfigRootFolder);

            ScenesManager.Instance.LoadPlacementScene();
        }

        private void Awake()
        {
            _SetFields();
        }
    }
}