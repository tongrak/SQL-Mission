using Assets.Scripts.BackendComponent.StepController;
using Assets.Scripts.Helper;
using Assets.Scripts.BackendComponent;
using Assets.Scripts.BackendComponent.Model;
using Assets.Scripts.BackendComponent.UI;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private GameObject _normalMission;
    [SerializeField] private GameObject _optionalMission;
    [SerializeField] private GameObject _finalMission;
    [SerializeField] private MissionData _missionSceneData;
    /// <summary>
    /// Path will be like this 'Assets/Resources/X/X/MissionConfigs/X/ChapterX'
    /// </summary>
    private string _allmissionConfigFolderPathFromAssets; // Insert path after root path. If relative path is './MissionConfig/Chapter1' then insert 'Chapter1'
    private string[] _missionConfigFiles; // list of mission config file. Example [mission1, mission2]

    public void MissionPaperClicked(string missionClickedFilename)
    {
        _missionSceneData.missionConfigFolderPathFromAssets = _allmissionConfigFolderPathFromAssets;
        _missionSceneData.missionFileName = missionClickedFilename;
        ScenesManager.Instance.LoadMissionScene();
    }

    public void Activate()
    {
        _LoadChapterConfigData();

        _GenMissionPaperFromConfigFiles();
    }

    /// <summary>
    /// Load chapter data from ScriptableObject
    /// </summary>
    private void _LoadChapterConfigData()
    {
        _missionConfigFiles = new string[]
        {
            "Mission1",
            "Mission2",
            "Mission3",
            "Mission4",
            "Mission5",
            "Final"
        };
        _allmissionConfigFolderPathFromAssets = "Assets/Resources/" + EnvironmentData.Instance.MissionConfigRootFolder + "/" + "Chapter1";
    }

    private void _GenMissionPaperFromConfigFiles()
    {
        // Set variable
        string missionConfigFolderPathAfterResources = _allmissionConfigFolderPathFromAssets.Split(new string[] { "Resources/" }, System.StringSplitOptions.None)[1] + '/';
        string unLockDetailFilePathFromAssets = _allmissionConfigFolderPathFromAssets + "/" + EnvironmentData.Instance.MissionStatusFileName; // Such as 'Assets/Resources/X/X/<FileName>'
        string missionStatusFileType = EnvironmentData.Instance.MissionStatusDetailFileType; // Can use '.txt' or '.json'. Up to you.
        bool haveStatusDetailFile = System.IO.File.Exists(unLockDetailFilePathFromAssets + missionStatusFileType);

        MissionConfig[] missionConfigs = new MissionConfig[_missionConfigFiles.Length];

        // Read each mission config file.
        for (int i = 0; i < _missionConfigFiles.Length; i++)
        {
            // Set config file path
            string missionConfigFilePathAfterResources = missionConfigFolderPathAfterResources + _missionConfigFiles[i];

            // Load config file.
            TextAsset configFiles = Resources.Load<TextAsset>(missionConfigFilePathAfterResources);
            missionConfigs[i] = JsonUtility.FromJson<MissionConfig>(configFiles.text);
        }

        // Get all status detail.
        MissionUnlockDetails missionUnlockDetails;
        if (!haveStatusDetailFile)
            {
            missionUnlockDetails = _WriteMissionUnlockDetails(missionConfigs, unLockDetailFilePathFromAssets, missionStatusFileType);
        }
        else
        {
            TextAsset unlockDetailsFile = Resources.Load<TextAsset>(missionConfigFolderPathAfterResources + EnvironmentData.Instance.MissionStatusFileName);
            missionUnlockDetails = JsonUtility.FromJson<MissionUnlockDetails>(unlockDetailsFile.text);
        }

        _GenerateAllMissionObject(missionConfigs, missionUnlockDetails);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="missionConfigs"></param>
    /// <param name="unLockDetailFilePathFromAssets">Example "Assets/Resources/X/X/ChapterX"</param>
    /// <param name="fileType">Must be ".txt" or ".json"</param>
    /// <returns></returns>
    private MissionUnlockDetails _WriteMissionUnlockDetails(MissionConfig[] missionConfigs, string unLockDetailFilePathFromAssets, string fileType)
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
        System.IO.File.WriteAllText(unLockDetailFilePathFromAssets+fileType, data);

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
            missionPaper.GetComponent<MissionPaperUI>().Initiate(missionConfig.MissionName, missionConfig.MissionDescription, missionUnlockDetail.IsUnlock, missionUnlockDetail.IsPass);

            // Injected MissionPaperController to mission paper.
            MissionPaperController missionPaperController = missionPaper.AddComponent<MissionPaperController>();
            missionPaperController.Construct(this, _missionConfigFiles[i]);
            missionPaper.GetComponent<Button>().onClick.AddListener(() => missionPaperController.MissionClicked());
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
