using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonController : MonoBehaviour
    {
        private ChapterButtonManager _chapterButtonManager;

        private string _missionConfigsRelativeFolder;
        private bool _isPassed;

        public void Construct(ChapterButtonManager chapterButtonManager, string missionConfigsRelativeFolder, bool isPassed)
        {
            _chapterButtonManager = chapterButtonManager; 
            _missionConfigsRelativeFolder = missionConfigsRelativeFolder;
            _isPassed = isPassed;
        }

        public void ChapterClicked()
        {
            _chapterButtonManager.ChapterButtonClicked(_missionConfigsRelativeFolder, _isPassed);
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