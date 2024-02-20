using Assets.Scripts.DataPersistence.StepController;
using Assets.Scripts.Helper;
using Assets.Scripts.DataPersistence;
using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.UI;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.InsideChapterLayer.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private GameObject _normalMission;
    [SerializeField] private GameObject _optionalMission;
    [SerializeField] private GameObject _finalMission;
    [SerializeField] private MissionData _missionSceneData;
    [SerializeField] private MissionStatusDetailsData _missionStatusDetailsData;
    [SerializeField] private MissionBoardUI _missionBoardUI;
    [SerializeField] private MissionBoardData _missionBoardData;
    /// <summary>
    /// Full path of config missions folder.
    /// </summary>
    private string _allmissionConfigFolderFullPath; // Insert path after root path. If relative path is './MissionConfig/Chapter1' then insert 'Chapter1'
    private string[] _missionConfigFiles; // list of mission config file. Example [mission1, mission2]
    private FileSystemWatcher _fileWatcher;
    private MissionUnlockDetails _missionStatusDetails;
    private bool _haveStatusDetailFile;

    public void MissionPaperClicked(int configIndex, bool isPassed)
    {
        //_missionSceneData.MissionFileName = missionClickedFilename;
        _missionSceneData.MissionConfigFolderFullPath = _allmissionConfigFolderFullPath;
        _missionSceneData.missionConfigIndex = configIndex;

        //_missionSceneData.IsPassed = isPassed;
        _missionStatusDetailsData.MissionStatusDetails = _missionStatusDetails;
        ScenesManager.Instance.LoadMissionScene();
    }

    public void Activate()
    {
        _LoadChapterConfigData();
        if (!_haveStatusDetailFile)
        {
            _InstantiateWatcher();
        }
        _InitiateMissionBoardUI();
        _GenMissionPaperFromConfigFiles();
    }

    /// <summary>
    /// Instantiate SystemFileWatcher to look at mission status file.
    /// </summary>
    private void _InstantiateWatcher()
    {
        _fileWatcher = new FileSystemWatcher(_allmissionConfigFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType);

        _fileWatcher.NotifyFilter = NotifyFilters.CreationTime
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Size;

        _fileWatcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Load chapter data from ScriptableObject
    /// </summary>
    private void _LoadChapterConfigData()
    {
        _missionConfigFiles = _missionBoardData.MissionFilesIndex;
        _allmissionConfigFolderFullPath = _missionBoardData.MissionConfigFolderFullPath;
        _haveStatusDetailFile = File.Exists(Path.Combine(_allmissionConfigFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType));
    }

    private void _GenMissionPaperFromConfigFiles()
    {
        // Set variable
        string unLockDetailFileFullPath = Path.Combine(_allmissionConfigFolderFullPath, EnvironmentData.Instance.StatusFileName);
        string missionStatusFileType = EnvironmentData.Instance.ConfigFileType;

        MissionConfig[] missionConfigs = new MissionConfig[_missionConfigFiles.Length];

        // Read each mission config file.
        for (int i = 0; i < _missionConfigFiles.Length; i++)
        {
            // Set config file path
            string missionConfigFileFullPathWithFormat = Path.Combine(_allmissionConfigFolderFullPath, _missionConfigFiles[i] + EnvironmentData.Instance.ConfigFileType);

            // Load config file.
            string configData = File.ReadAllText(missionConfigFileFullPathWithFormat);
            missionConfigs[i] = JsonUtility.FromJson<MissionConfig>(configData);
        }

        // Load all mission status detail.
        if (_missionStatusDetailsData.Changed)
        {
            _missionStatusDetails = _missionStatusDetailsData.MissionStatusDetails;
            _missionStatusDetailsData.Changed = false;
        }
        else
        {
            if (!_haveStatusDetailFile)
            {
                _missionStatusDetails = _WriteMissionUnlockDetails(missionConfigs, unLockDetailFileFullPath, missionStatusFileType);
            }
            else
            {
                string missionStatusTxt = File.ReadAllText(Path.Combine(_allmissionConfigFolderFullPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType));
                _missionStatusDetails = JsonUtility.FromJson<MissionUnlockDetails>(missionStatusTxt);
            }
        }

        _SaveConfigsToSO(missionConfigs);
        _GenerateAllMissionObject(missionConfigs, _missionStatusDetails);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="missionConfigs"></param>
    /// <param name="unLockDetailFileFullPath">Example "D:/Assets/Resources/X/X/ChapterX"</param>
    /// <param name="fileType">Must be ".txt" or ".json"</param>
    /// <returns></returns>
    private MissionUnlockDetails _WriteMissionUnlockDetails(MissionConfig[] missionConfigs, string unLockDetailFileFullPath, string fileType)
    {
        MissionUnlockDetails missionUnlockDetails = new MissionUnlockDetails(missionConfigs.Length);

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
            missionUnlockDetails.MissionUnlockDetailList[i] = new MissionUnlockDetail
            {
                MissionID = missionConfig.MissionID,
                IsUnlock = isMissionUnlocked,
                IsPass = false,
                MissionDependenciesUnlockDetail = missionDependenciesUnlockDetail
            };
        }

        // Save to 'UnlockDetail.txt'
        string data = JsonUtility.ToJson(missionUnlockDetails, true);

        File.WriteAllText(unLockDetailFileFullPath+fileType, data);

        return missionUnlockDetails;
    }

    /// <summary>
    /// Generate all mission paper to scene.
    /// </summary>
    /// <param name="missionConfigs"></param>
    /// <param name="missionUnlockDetails"></param>
    private void _GenerateAllMissionObject(MissionConfig[] missionConfigs, MissionUnlockDetails missionUnlockDetails)
    {
        // Find parent's transform
        Transform misionGroupTransform = GameObject.Find("Content").transform;

        // Pair missionID and missionTitle
        IDictionary<int, string> missionDic = missionConfigs.ToDictionary(x => x.MissionID, x => x.MissionTitle);

        // Instantiate mission(s)
        for (int i = 0; i < missionConfigs.Length; i++)
        {
            MissionConfig missionConfig = missionConfigs[i];
            MissionUnlockDetail missionUnlockDetail = missionUnlockDetails.MissionUnlockDetailList[i];
            GameObject missionPaper = null;

            switch (missionConfig.MissionType)
            {
                case MissionType.Normal:
                    missionPaper = Instantiate(_normalMission, misionGroupTransform);
                    break;
                case MissionType.Optional:
                    missionPaper = Instantiate(_optionalMission, misionGroupTransform);
                    break;
                case MissionType.Final:
                    missionPaper = Instantiate(_finalMission, misionGroupTransform);
                    break;
                default:
                    break;
            }

            // Injected MissionPaperUI to mission paper.
            string[] missionDependencyTitles = _MapMissionIDToTitle(missionConfig.MissionDependencies, missionDic);
            missionPaper.GetComponent<MissionPaperUI>().Initiate(missionConfig.MissionTitle, missionConfig.MissionDescription, missionUnlockDetail.IsUnlock, missionUnlockDetail.IsPass, missionDependencyTitles);

            // Injected MissionPaperController to mission paper.
            MissionPaperController missionPaperController = missionPaper.AddComponent<MissionPaperController>();
            missionPaperController.Construct(this, i, missionUnlockDetail.IsPass);
            missionPaper.GetComponent<Button>().onClick.AddListener(() => missionPaperController.MissionClicked());
        }
    }

    private string[] _MapMissionIDToTitle(int[] missionIDs, IDictionary<int, string> missions)
    {
        string[] missionTitles = new string[missionIDs.Length];

        for (int i = 0; i < missionIDs.Length; i++)
        {
            missionTitles[i] = missions[missionIDs[i]];
        }

        return missionTitles;
    }

    private void _InitiateMissionBoardUI()
    {
        _missionBoardUI.Initiate(_fileWatcher);
    }

    private void _SaveConfigsToSO(MissionConfig[] missionConfigs)
    {
        _missionSceneData.missionConfigs = missionConfigs;
    }

    // Start is called before the first frame update
    void Start()
    {
        Activate();
    }

    #region Object disable or destroy
    private void OnDisable()
    {
        _DisposeWatcher();
    }

    private void OnDestroy()
    {
        _DisposeWatcher();
    }

    private void _DisposeWatcher()
    {
        if (_fileWatcher != null)
        {
            _fileWatcher.Dispose();
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {

    }
}
