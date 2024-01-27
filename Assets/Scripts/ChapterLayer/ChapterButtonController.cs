using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonController : MonoBehaviour
    {
        [SerializeField] private ChapterButtonManager _chapterManager;

        private string _missionConfigsRelativeFolder;
        private bool _isPassed;

        public void Construct(string missionConfigsRelativeFolder, bool isPassed)
        {
            _missionConfigsRelativeFolder = missionConfigsRelativeFolder;
            _isPassed = isPassed;
        }

        public void ChapterClicked()
        {
            //_chapterManager.ChapterButtonClicked(_missionConfigsRelativeFolder, _isPassed);
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