using Assets.Scripts.BackendComponent.SaveManager;
using Assets.Scripts.BackendComponent.StepController;
using System;
using UnityEngine;

namespace Assets.Scripts.BackendComponent
{
    public class MissionController : MonoBehaviour
    {
        private MissionType _missionType;
        private string _missionName;
        /// <summary>
        /// Must be like 'Assets/Resources/X/XMissionConfigs/X/ChapterX'
        /// </summary>
        private string _missionFolderPath;
        private string[] _missionDependTos;
        private ISaveManager _saveManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missionConfigFolderPath">Path must start with 'Assets' then follow by 'Resources'. Example 'Assets/Resources/X/XMissionConfigs/X/ChapterX'</param>
        /// <param name="missionName"></param>
        /// <param name="missionDependTos"></param>
        /// <param name="missionType"></param>
        /// <param name="saveManager"></param>
        public void Initiate(string missionConfigFolderPath, string missionName, string[] missionDependTos,MissionType missionType, ISaveManager saveManager)
        {
            _missionFolderPath = missionConfigFolderPath;
            _missionName = missionName;
            _missionType = missionType;
            _saveManager = saveManager;
            _missionDependTos = missionDependTos;
        }

        public void AllPuzzlePassed()
        {
            if(_missionType != MissionType.Final)
            {
                _saveManager.UpdateMissionStatus(_missionFolderPath.Split(new string[] {"Resources/"}, StringSplitOptions.None)[1], _missionName, _missionDependTos);
            }
            else
            {
                // Update chapter status.
            }
        }
    }
}