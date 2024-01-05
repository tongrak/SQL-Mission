using Assets.Scripts.InsideChapterLayer.Model;
using Assets.Scripts.InsideChapterLayer.UI;
using Assets.Scripts.MissionGenComponent.Model;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private GameObject _normalMission;
    [SerializeField] private GameObject _optionalMission;
    [SerializeField] private GameObject _finalMission;
    private string _missionFolderPath; // Insert path after root path. If relative path is './MissionConfig/Chapter1' then insert 'Chapter1'
    private string[] _missionConfigFiles;
    //private MissionPaperDetail[] _missionPapersDetail;

    /// <summary>
    /// Set list of mission config file. Example [mission1.txt, mission2.txt]
    /// </summary>
    /// <param name="missions">List of mission config file</param>
    public void SetMissionConfigFiles(string[] missions)
    {
        _missionConfigFiles = missions;
    }

    /// <summary>
    /// Set relative folder path for mission config files.
    /// </summary>
    /// <param name="path">Insert path after root path. If relative path is './MissionConfig/Chapter1' then insert 'Chapter1'</param>
    public void SetMissionFolderPath(string path)
    {
        _missionFolderPath = path;
    }

    public void Activate()
    {
        string mockMissionFolderPath = "Chapter1";
        string[] mockMissionFiles = new string[]
        {
            "Mission1",
            "Mission2",
            "Mission3",
            "Mission4",
            "Mission5",
            "Final"
        };

        SetMissionFolderPath(mockMissionFolderPath);

        SetMissionConfigFiles(mockMissionFiles);

        _GenMissionPaperFromConfigFiles();
        //_MockGenerateMissionObject(5);
    }

    private async void _GenMissionPaperFromConfigFiles()
    {
        string missionConfigFolderPath = "MissionConfigs/" + _missionFolderPath + '/';
        string unlockDetailFile = "UnlockDetail";
        string unLockDetailPathAfterResources = missionConfigFolderPath + unlockDetailFile;
        string unLockDetailPathFromAssets = "Assets/Resources/" + unLockDetailPathAfterResources;
        bool haveUnlockDetail = System.IO.File.Exists(unLockDetailPathFromAssets);

        //_missionPapersDetail = new MissionPaperDetail[_missionConfigFiles.Length];

        MissionConfig[] missionConfigs = new MissionConfig[_missionConfigFiles.Length];

        // Read each mission config file.
        for (int i = 0; i < _missionConfigFiles.Length; i++)
        {
            // Set config file path
            string missionConfigFilePath = missionConfigFolderPath + _missionConfigFiles[i];

            // Load config file.
            TextAsset configFiles = Resources.Load<TextAsset>(missionConfigFilePath);
            missionConfigs[i] = JsonUtility.FromJson<MissionConfig>(configFiles.text);
        }

        MissionUnlockDetails missionUnlockDetails;

        if (!haveUnlockDetail)
        {
            missionUnlockDetails = _WriteMissionUnlockDetails(missionConfigs, unLockDetailPathFromAssets, _missionConfigFiles.Length);
        }
        else
        {
            TextAsset unlockDetailsFile = Resources.Load<TextAsset>(unLockDetailPathAfterResources);
            missionUnlockDetails = JsonUtility.FromJson<MissionUnlockDetails>(unlockDetailsFile.text);
        }

        _GenerateAllMissionObject(missionConfigs, missionUnlockDetails);
    }

    private MissionUnlockDetails _WriteMissionUnlockDetails(MissionConfig[] missionConfigs, string unLockDetailPathFromAssets, int configFileNum)
    {
        int missionNum = configFileNum;
        string fileType = ".json";
        MissionUnlockDetails missionUnlockDetails = new MissionUnlockDetails(missionNum);

        // Create unlock detail for each mission.
        for (int i = 0; i < missionNum; i++)
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
                        MissionName = missionConfig.MissionDependencies[k],
                        IsPass = false
                    };
                }
            }

            // Create unlock detail
            missionUnlockDetails.MissionUnlockDetailList[i] = new MissionUnlockDetail
            {
                MissionName = missionConfig.MissionName,
                IsUnlock = isMissionUnlocked,
                IsPass = false,
                MissionDependenciesUnlockDetail = missionDependenciesUnlockDetail
            };
        }

        // Save to 'UnlockDetail.txt'
        string data = JsonUtility.ToJson(missionUnlockDetails);
        System.IO.File.WriteAllText(unLockDetailPathFromAssets+fileType, data);

        return missionUnlockDetails;
    }

    private void _GenerateAllMissionObject(MissionConfig[] missionConfigs, MissionUnlockDetails missionUnlockDetails)
    {
        // Find parent's transform
        Transform misionGroupTransform = GameObject.Find("Content").transform;


        // Instantiate mission(s)
        for (int i = 0; i < missionConfigs.Length; i++)
        {
            MissionConfig missionConfig = missionConfigs[i];
            MissionUnlockDetail missionUnlockDetail = missionUnlockDetails.MissionUnlockDetailList[i];

            switch (missionConfig.MissionType)
            {
                case MissionType.Normal:
                    Instantiate(_normalMission, misionGroupTransform).GetComponent<MissionPaperUI>().Initiate(missionConfig.MissionName, missionConfig.MissionDescription, missionUnlockDetail.IsUnlock, missionUnlockDetail.IsPass);
                    break;
                case MissionType.Optional:
                    Instantiate(_optionalMission, misionGroupTransform).GetComponent<MissionPaperUI>().Initiate(missionConfig.MissionName, missionConfig.MissionDescription, missionUnlockDetail.IsUnlock, missionUnlockDetail.IsPass);
                    break;
                case MissionType.Final:
                    Instantiate(_finalMission, misionGroupTransform).GetComponent<MissionPaperUI>().Initiate(missionConfig.MissionName, missionConfig.MissionDescription, missionUnlockDetail.IsUnlock, missionUnlockDetail.IsPass);
                    break;
                default:
                    break;
            }
        }
    }

    private void _MockGenerateMissionObject(int missionNum)
    {
        // Find parent's transform
        Transform misionGroupTransform = GameObject.Find("Content").transform;

        string mockDescription = "This is description blallfldsfljslfdjfjajdlasdjflka";
        string mockTitle = "Mission2";
        bool mockIsUnlock = false;

        // Instantiate mission(s)
        for (int i = 0; i < missionNum; i++)
        {
            Instantiate(_normalMission, misionGroupTransform).GetComponent<MissionPaperUI>().Initiate(mockTitle, mockDescription, mockIsUnlock, mockIsUnlock);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
