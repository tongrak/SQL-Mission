using Assets.Scripts.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterGenerator : MonoBehaviour
{
    [SerializeField] private Transform _ChaptersContainer;

    private string _allChapterConfigFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.ChapterConfigRootFolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
