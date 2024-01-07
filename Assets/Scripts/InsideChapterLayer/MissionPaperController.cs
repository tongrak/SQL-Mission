using UnityEngine;

namespace Assets.Scripts.InsideChapterLayer
{
    public class MissionPaperController : MonoBehaviour
    {
        private string _missionConfigPath;
        [SerializeField] private MissionManager _missionManager;

        /// <summary>
        /// Construct mission controller completely.
        /// </summary>
        /// <param name="missionConfigFilePath">Path must be like this 'MissionConfigs/ChapterX/missionName' and path must after 'Resoruces' folder.</param>
        public void Construct(MissionManager missionManager ,string missionConfigFilePath)
        {
            _missionManager = missionManager;
            _missionConfigPath = missionConfigFilePath;
        }

        public void MissionClicked()
        {
            _missionManager.MissionPaperClicked(_missionConfigPath);
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