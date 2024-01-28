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

        private ChapterStatusDetails _chapterStatusDetails;
        private string[] _chapterFileIndex;

        public void ChapterButtonClicked(string missionConfigsRelativeFolder, bool isPassed)
        {
            _missionBoardData.MissionConfigFolderFullPath = Application.dataPath + "/Resources/" + EnvironmentData.Instance.MissionConfigRootFolder + "/" + missionConfigsRelativeFolder;
            _missionBoardData.ChapterFileIndex = _chapterFileIndex;
            _missionBoardData.IsPassed = isPassed;
            _chapterStatusDetailsData.ChapterStatusDetails = _chapterStatusDetails;
            ScenesManager.Instance.LoadSelectMissionScene();
        }

        public void Construct(string[] chapterFileIndex ,ChapterStatusDetails chapterStatusDetails)
        {
            _chapterStatusDetails = chapterStatusDetails;
            _chapterFileIndex = chapterFileIndex;
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