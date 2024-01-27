using UnityEngine;

namespace Assets.Scripts.ChapterLayer
{
    public class ChapterButtonManager : MonoBehaviour
    {

        public void ChapterButtonClicked(string missionConfigsRelativeFolder, bool isPassed)
        {
            //_missionSceneData.MissionConfigFolderFullPath = _allmissionConfigFolderFullPath;
            //_missionSceneData.MissionFileName = missionConfigsRelativeFolder;
            //_missionSceneData.IsPassed = isPassed;
            //_missionStatusDetailsData.MissionStatusDetails = _missionStatusDetails;
            ScenesManager.Instance.LoadSelectMissionScene();
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