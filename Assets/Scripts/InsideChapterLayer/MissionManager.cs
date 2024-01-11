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
    private string _allmissionConfigFolderPath; // Insert path after root path. If relative path is './MissionConfig/Chapter1' then insert 'Chapter1'
    private string[] _missionConfigFiles; // list of mission config file. Example [mission1, mission2]

    public void MissionPaperClicked(string missionClickedFilename)
    {
        _missionSceneData.missionConfigFolderPath = _allmissionConfigFolderPath;
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
        _allmissionConfigFolderPath = EnvironmentData.Instance.MissionConfigRootFolder + "/" + "Chapter1";
    }

    private void _GenMissionPaperFromConfigFiles()
    {
        // Set variable
        string missionConfigFolderPath = _allmissionConfigFolderPath + '/';
        string unlockDetailFile = "UnlockDetail";
        string unLockDetailPathAfterResources = missionConfigFolderPath + unlockDetailFile;
        string unLockDetailPathFromAssets = "Assets/Resources/" + unLockDetailPathAfterResources;
        bool haveStatusDetail = System.IO.File.Exists(unLockDetailPathFromAssets);

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

        // Get load status detail.
        MissionUnlockDetails missionUnlockDetails;
        if (!haveStatusDetail)
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
