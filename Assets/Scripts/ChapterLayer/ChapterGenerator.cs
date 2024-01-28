using Assets.Scripts.ChapterLayer;
using Assets.Scripts.ChapterLayer.UI;
using Assets.Scripts.ChapterLayer.Model;
using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.Helper;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ChapterGenerator : MonoBehaviour
{
    [SerializeField] private Transform _chaptersContainer;
    [SerializeField] private GameObject _chapterPrefab;
    [SerializeField] private MissionBoardData _missionBoardData;
    [SerializeField] private ChapterButtonManager _chapterManager;

    private string _chapterConfigsFolderFullPath; 
    private string _chapterStatusFileFullPath;
    private bool _haveStatusFile;

    private void Activate()
    {
        // 1) Set fields.
        _SetFields();

        // 2) Load config from file by use config index
        ChapterIndex chapterIndex = _LoadChapterIndexFromFile();
        ChapterConfig[] chapterConfigs = _LoadChapterConfigsFromFile(chapterIndex);

        // 3) Load chapter status.
        ChapterStatusDetails chapterStatusDetails;
        if (!_haveStatusFile)
        {
            chapterStatusDetails = _WriteChapterStatusDetailFile(chapterConfigs);
        }
        else
        {
            chapterStatusDetails = _LoadChapterStatusDetailsFromFile();
        }

        // 4) Generate chapter button to UI.
        _GenerateChapterObjects(chapterConfigs, chapterStatusDetails);

        // 5) Init chapter button manager.
        _chapterManager.Construct(chapterStatusDetails);
    }

    private void _SetFields()
    {
        _chapterConfigsFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.ChapterConfigRootFolder; ;
        _chapterStatusFileFullPath = _chapterConfigsFolderFullPath + "/" + EnvironmentData.Instance.StatusFileName;
        _haveStatusFile = File.Exists(_chapterStatusFileFullPath + EnvironmentData.Instance.ConfigFileType);
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

    private ChapterStatusDetails _LoadChapterStatusDetailsFromFile()
    {
        string chapterStatusData = File.ReadAllText(_chapterStatusFileFullPath + EnvironmentData.Instance.ConfigFileType);
        ChapterStatusDetails chapterStatusDetails = JsonUtility.FromJson<ChapterStatusDetails>(chapterStatusData);
        return chapterStatusDetails;
    }

    private ChapterStatusDetails _WriteChapterStatusDetailFile(ChapterConfig[] chapterConfigs)
    {
        ChapterStatusDetails chapterStatusDetails = new ChapterStatusDetails(chapterConfigs.Length);

        // Create status detail for each chapter.
        for (int i = 0; i < chapterConfigs.Length;i++)
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
        string data = JsonUtility.ToJson(chapterStatusDetails);

        File.WriteAllText(_chapterStatusFileFullPath + EnvironmentData.Instance.ConfigFileType, data);

        return chapterStatusDetails;
    }

    private void _GenerateChapterObjects(ChapterConfig[] chapterConfigs, ChapterStatusDetails chapterStatusDetails)
    {
        // Pair chapterID and chapterTitle
        IDictionary<int, string> chapterDict = chapterConfigs.ToDictionary(x => x.ChapterID, x => x.ChapterTitle);

        // Instantiate chapter(s)
        for (int i = 0; i < chapterConfigs.Length; i++)
        {
            ChapterConfig chapterConfig = chapterConfigs[i];
            ChapterStatusDetail chapterStatusDetail = chapterStatusDetails.ChapterStatusDetailList[i];
            GameObject chapterButton = Instantiate(_chapterPrefab, _chaptersContainer);

            // Init ChapterButtonUI
            string[] chapterDependencyTitles = _MapChapterIDToTitle(chapterConfig.ChapterDependencies, chapterDict);
            chapterButton.GetComponent<ChapterButtonUI>().Initiate(chapterConfig.ChapterTitle, chapterStatusDetail.IsUnlock, chapterStatusDetail.IsPass, chapterDependencyTitles);

            // Init ChapterButtonController
            chapterButton.GetComponent<ChapterButtonController>().Construct(_chapterManager, chapterConfig.MissionConfigFolder, chapterStatusDetail.IsPass, chapterConfig.MissionFilesIndex);
        }
    }

    private string[] _MapChapterIDToTitle(int[] chapterIDs, IDictionary<int, string> chapterDict)
    {
        string[] chapterTitles = new string[chapterIDs.Length];

        for (int i = 0; i < chapterIDs.Length; i++)
        {
            chapterTitles[i] = chapterDict[chapterIDs[i]];
        }

        return chapterTitles;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
