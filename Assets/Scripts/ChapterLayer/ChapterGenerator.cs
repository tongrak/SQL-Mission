using Assets.Scripts.ChapterLayer.Model;
using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.Helper;
using System.IO;
using UnityEngine;

public class ChapterGenerator : MonoBehaviour
{
    [SerializeField] private Transform _ChaptersContainer;
    [SerializeField] private GameObject _ChapterPrefab;

    private string _chapterConfigsFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.ChapterConfigRootFolder;
    private string _chapterStatusFileFullPath;
    private bool _haveStatusFile;

    private void Activate()
    {
        // 1) Set fields.
        _chapterStatusFileFullPath = _chapterConfigsFolderFullPath + "/" + EnvironmentData.Instance.StatusFileName;
        _haveStatusFile = File.Exists(_chapterStatusFileFullPath + EnvironmentData.Instance.ConfigFileType);

        // 2) Load config from file by use config index
        string chapterIndexData = File.ReadAllText(_chapterConfigsFolderFullPath + "/" + EnvironmentData.Instance.ChpaterFileIndexFileName + EnvironmentData.Instance.ConfigFileType);
        ChapterIndex chapterIndex = JsonUtility.FromJson<ChapterIndex>(chapterIndexData);
        ChapterConfig[] chapterConfigs = new ChapterConfig[chapterIndex.ChapterFileIndex.Length];
        for (int i = 0; i < chapterConfigs.Length; i ++)
        {
            string chapterFileName = chapterIndex.ChapterFileIndex[i];
            string chapterConfigData = File.ReadAllText(_chapterConfigsFolderFullPath + "/" + chapterFileName + EnvironmentData.Instance.ConfigFileType);
            chapterConfigs[i] = JsonUtility.FromJson<ChapterConfig>(chapterConfigData);
        }

        // 3) Load chapter status.
        if (!_haveStatusFile)
        {

        }
        else
        {
            string chapterStatusData = File.ReadAllText(_chapterStatusFileFullPath + EnvironmentData.Instance.ConfigFileType);
            ChapterStatusDetails chapterStatusDetails = JsonUtility.FromJson<ChapterStatusDetails>(chapterStatusData);
        }
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
                ChatperID = chapterConfig.ChpaterID,
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
