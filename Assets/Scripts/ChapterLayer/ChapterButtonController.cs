using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonController : MonoBehaviour
    {
        private ChapterButtonManager _chapterButtonManager;

        private string _missionConfigsRelativeFolder;
        private bool _isPassed;
        private string[] _missionFilesIndex;
        private int _chapterID;

        public void Construct(ChapterButtonManager chapterButtonManager, int chapterID ,string missionConfigsRelativeFolder, bool isPassed, string[] missionFilesIndex)
        {
            _chapterButtonManager = chapterButtonManager; 
            _missionConfigsRelativeFolder = missionConfigsRelativeFolder;
            _isPassed = isPassed;
            _missionFilesIndex = missionFilesIndex;
            _chapterID = chapterID;
        }

        public void ChapterClicked()
        {
            _chapterButtonManager.ChapterButtonClicked(_chapterID, _missionConfigsRelativeFolder, _isPassed, _missionFilesIndex);
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