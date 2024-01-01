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
    private MissionPaperDetail[] _missionPapersDetail;

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
            "Mission1.txt",
            "Mission2.txt",
            "Mission3.txt",
            "Mission4.txt",
            "Mission5.txt",
            "Final.txt"
        };

        SetMissionFolderPath(mockMissionFolderPath);

        SetMissionConfigFiles(mockMissionFiles);

        _LoadDataFromConfigFiles();
        //_GenerateMissionObject(5);
    }

    private void _LoadDataFromConfigFiles()
    {
        _missionPapersDetail = new MissionPaperDetail[_missionConfigFiles.Length];
        for (int i = 0; i < _missionConfigFiles.Length; i++)
        {
            string mission = _missionConfigFiles[i];
            string missionConfigFolderPath = "MissionConfigs/" + _missionFolderPath + '/';
            string missionConfigFilePath = missionConfigFolderPath + mission;

            // Load config file.
            TextAsset configFiles = Resources.Load<TextAsset>(missionConfigFilePath);
            MissionConfig missionConfig = JsonUtility.FromJson<MissionConfig>(configFiles.text);

            // Load data to mission manager.
            _missionPapersDetail[i].MissionName = missionConfig.MissionName;

            /* ขั้นตอนการ Generate mission paper
             * 1) Check ว่ามีไฟล์ JSON ที่เก็บข้อมูลการปลดล็อกด่านไว้หรือไม่
             *  1A) ถ้าไม่มีให้ gen ขึ้นมาใหม่แล้วให้ใส่ข้อมูลการปลดล็อกของด่านด้วย พร้อมกับดึงข้อมูลออกมา
             *      - รายละเอียดการสร้างนั้นจะไปสร้างที่ folder เดียวกับ mission config
             *      - โดย file ที่สร้างจะมีชื่อว่า 'UnlockDetail.txt'
             *  1B) ถ้าหากมีอยู่แล้วให้ดึงข้อมูลออกมา
             * 2) ให้ทำการสร้าง mission paper โดยอิงตามข้อมูลการปลดล็อกของ mission นั้น ๆ
             *
            */
            string unlockDetailFile = "UnlockDetail.txt";
            string unLockDetailPath = "Assets/Resources/" + missionConfigFolderPath + unlockDetailFile;
            if (System.IO.File.Exists(unLockDetailPath))
            {
                int missionNum = _missionConfigFiles.Length;
                MissionUnlockDetails missionUnlockDetails = new MissionUnlockDetails(missionNum);

                for(int j = 0; j < missionNum; j++)
                {
                    int missionDependencyNum = missionConfig.MissionDependencies.Length;
                    bool isUnlock = true;
                    MissionDependencyUnlockDetail[] missionDependenciesUnlockDetail = null;
                    if (missionDependencyNum > 0)
                    {
                        isUnlock = false;
                        missionDependenciesUnlockDetail = new MissionDependencyUnlockDetail[missionDependencyNum];
                        for(int k = 0; k < missionDependencyNum; k++)
                        {
                            missionDependenciesUnlockDetail[k] = new MissionDependencyUnlockDetail
                            {
                                MissionName = missionConfig.MissionDependencies[k],
                                IsUnlock = false
                            };
                        }
                    }

                    missionUnlockDetails.MissionUnlockDetailList[j] = new MissionUnlockDetail
                    {
                        MissionName = missionConfig.MissionName,
                        IsUnlock = isUnlock,
                        MissionDependenciesUnlockDetail = missionDependenciesUnlockDetail
                    };
                }
                

                string data = JsonUtility.ToJson(missionUnlockDetails);
                System.IO.File.WriteAllText(unLockDetailPath, data);
                Debug.Log("Have unlock detail file");
            }
            else
            {
                Debug.Log("Don't have unlock detail file");
            }

    }
}

    private void _GenerateMissionObject(int missionNum)
    {
        // Find parent's transform
        Transform misionGroupTransform = GameObject.Find("Content").transform;

        string mockDescription = "This is description blallfldsfljslfdjfjajdlasdjflka";
        string mockTitle = "Mission2";
        bool mockIsPass = false;

        // Instantiate mission(s)
        for (int i = 0; i < missionNum; i++)
        {
            Instantiate(_normalMission, misionGroupTransform).GetComponent<MissionPaperUI>().Initiate(mockTitle, mockDescription, mockIsPass);
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
