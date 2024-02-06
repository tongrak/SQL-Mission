using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.Helper;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonManager : MonoBehaviour
    {
        [SerializeField] private MissionBoardData _missionBoardData;
        [SerializeField] private ChapterStatusDetailsData _chapterStatusDetailsData;
        [SerializeField] private SelectedChapterData _selectedChapterData;

        private ChapterStatusDetails _chapterStatusDetails;
        private string _chapterConfigsFolderFullPath;

        public void ChapterButtonClicked(int chapterID, string missionConfigsRelativeFolder, bool isPassed, string[] missionFilesIndex)
        {
            // Insert mission board data
            _missionBoardData.MissionConfigFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.MissionConfigRootFolder + "/" + missionConfigsRelativeFolder;
            _missionBoardData.MissionFilesIndex = missionFilesIndex;

            // Insert chapter status data
            _chapterStatusDetailsData.ChapterStatusDetails = _chapterStatusDetails;

            // Insert chapter data
            _selectedChapterData.ChapterFolderFullPath = _chapterConfigsFolderFullPath;
            _selectedChapterData.ChapterID = chapterID;
            _selectedChapterData.IsPassed = isPassed;

            ScenesManager.Instance.LoadSelectMissionScene();
        }

        public void Construct(string chapterConfigsFolderFullPath, ChapterStatusDetails chapterStatusDetails)
        {
            _chapterStatusDetails = chapterStatusDetails;
            _chapterConfigsFolderFullPath = chapterConfigsFolderFullPath;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}