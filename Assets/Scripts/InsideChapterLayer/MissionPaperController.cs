using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class MissionPaperController : MonoBehaviour
    {
        private int _missionConfigIndex;
        private bool _isPassed;
        private MissionManager _missionManager;

        /// <summary>
        /// Construct mission controller completely.
        /// </summary>
        /// <param name="missionConfigIndex"></param>
        /// <param name="isPassed"></param>
        public void Construct(MissionManager missionManager, int missionConfigIndex, bool isPassed)
        {
            _missionManager = missionManager;
            _missionConfigIndex = missionConfigIndex;
            _isPassed = isPassed; 
        }

        public void MissionClicked()
        {
            _missionManager.MissionPaperClicked(_missionConfigIndex, _isPassed);
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