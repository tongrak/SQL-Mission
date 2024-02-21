using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using Assets.Scripts.Helper;
using Assets.Scripts.ScriptableObjects;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonManager : MonoBehaviour
    {
        [SerializeField] private MissionBoardData _missionBoardData;
        [SerializeField] private SelectedChapterData _selectedChapterData;
        [SerializeField] private GameObject _popup;
        [SerializeField] private string _url;

        private string _chapterConfigsFolderFullPath;

        public void ChapterButtonClicked(int chapterID, string missionConfigsRelativeFolder, bool isPassed, string[] missionFilesIndex)
        {
            // Insert mission board data
            _missionBoardData.MissionConfigFolderFullPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.MissionConfigRootFolder, missionConfigsRelativeFolder);
            _missionBoardData.MissionFilesIndex = missionFilesIndex;

            // Insert chapter data
            _selectedChapterData.ChapterFolderFullPath = _chapterConfigsFolderFullPath;
            _selectedChapterData.ChapterID = chapterID;
            _selectedChapterData.IsPassed = isPassed;

            ScenesManager.Instance.LoadSelectMissionScene();
        }

        public void Construct(string chapterConfigsFolderFullPath)
        {
            _chapterConfigsFolderFullPath = chapterConfigsFolderFullPath;
        }

        public void URLButtonClicked()
        {
            _popup?.SetActive(true);
        }

        public void ConfirmURLButtonClicked()
        {
            Application.OpenURL(_url);
            Application.Quit();
        }

        public void CancelURLButtonClicked()
        {
            _popup?.SetActive(false);
        }
    }
}