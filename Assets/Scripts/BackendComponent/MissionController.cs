using Assets.Scripts.BackendComponent.SaveManager;
using Assets.Scripts.BackendComponent.StepController;
using UnityEngine;

namespace Assets.Scripts.BackendComponent
{
    public class MissionController : MonoBehaviour
    {
        private MissionType _missionType;
        private string _missionName;
        private string _missionFolderPath; // Must be like this 'MissionConfigs/ChapterX'
        private ISaveManager _saveManager;

        public void Initiate(string missionConfigFolderPath, string missionName, MissionType missionType, ISaveManager saveManager)
        {
            _missionFolderPath = missionConfigFolderPath;
            _missionName = missionName;
            _missionType = missionType;
            _saveManager = saveManager;
        }

        public void AllPuzzlePassed()
        {
            if(_missionType != MissionType.Final)
            {
                _saveManager.UpdateMissionStatus(_missionFolderPath, _missionName);
            }
            else
            {
                // Update chapter status.
            }
        }
    }
}