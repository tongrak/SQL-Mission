using Assets.Scripts.ChapterLayer;
using Assets.Scripts.ChapterLayer.UI;
using Assets.Scripts.DataPersistence.Chapter;
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
    [SerializeField] private ChapterStatusDetailsData _chapterStatusDetailsData;

    private string _chapterConfigsFolderFullPath; 

    private void Activate()
    {
        // 1) Set fields.
        _SetFields();

        // 2) Load config from file by use config index
        ChapterIndex chapterIndex = _LoadChapterIndexFromFile();
        ChapterConfig[] chapterConfigs = _LoadChapterConfigsFromFile(chapterIndex);

        // 3) Generate chapter(s) to UI.
        _GenerateChapterObjects(chapterConfigs, _chapterStatusDetailsData.ChapterStatusDetails);

        // 4) Init chapter manager.
        _chapterManager.Construct(_chapterConfigsFolderFullPath);
    }

    private void _SetFields()
    {
        _chapterConfigsFolderFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.ChapterConfigRootFolder);
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
            chapterButton.GetComponent<ChapterButtonUI>().Initiate(chapterConfig.ChapterTitle, chapterStatusDetail.IsUnlock, i+1, chapterStatusDetail.IsPass, chapterDependencyTitles);

            // Init ChapterButtonController
            chapterButton.GetComponent<ChapterButtonController>().Construct(_chapterManager, chapterConfig.ChapterID, chapterConfig.MissionConfigFolder, chapterStatusDetail.IsPass, chapterConfig.MissionFilesIndex);
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
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
