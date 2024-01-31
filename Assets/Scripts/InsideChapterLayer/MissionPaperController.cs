using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class MissionPaperController : MonoBehaviour
    {
        private string _missionFileName;
        private bool _isPassed;
        private MissionManager _missionManager;

        /// <summary>
        /// Construct mission controller completely.
        /// </summary>
        /// <param name="missionConfigFilePath">Path must be like this 'MissionConfigs/ChapterX' and path must after 'Resources' folder.</param>
        /// <param name="missionFileName"></param>
        public void Construct(MissionManager missionManager, string missionFileName, bool isPassed)
        {
            _missionManager = missionManager;
            _missionFileName = missionFileName;
            _isPassed = isPassed; 
        }

        public void MissionClicked()
        {
            _missionManager.MissionPaperClicked(_missionFileName, _isPassed);
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