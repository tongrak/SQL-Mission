﻿using Assets.Scripts.DataPersistence.SaveManager;
using Assets.Scripts.DataPersistence.StepController;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private GameObject _stepControllerGameObject;
        [SerializeField] private MissionStatusDetailsData _missionStatusDetailsData;
        
        private MissionType _missionType;
        private string _missionName;
        /// <summary>
        /// Must be like 'Assets/Resources/X/XMissionConfigs/X/ChapterX'
        /// </summary>
        private string _missionFolderFullPath;
        private string[] _missionDependTos;
        private ISaveManager _saveManager;
        private bool _isPassed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missionConfigFolderFullPath">Such as "D:/RootFolder/Assets/Resources/X/X/Chapter1"</param>
        /// <param name="missionName"></param>
        /// <param name="missionDependTos"></param>
        /// <param name="missionType"></param>
        /// <param name="saveManager"></param>
        public void Initiate(string missionConfigFolderFullPath, string missionName, string[] missionDependTos,MissionType missionType, ISaveManager saveManager, bool isPassed)
        {
            _missionFolderFullPath = missionConfigFolderFullPath;
            _missionName = missionName;
            _missionType = missionType;
            _saveManager = saveManager;
            _missionDependTos = missionDependTos;
            _isPassed = isPassed;
        }

        public void AllPuzzlePassed()
        {
            if (!_isPassed)
            {
                if (_missionType != MissionType.Final)
                {
                    _missionStatusDetailsData.MissionStatusDetails = _saveManager.UpdateMissionStatus(_missionFolderFullPath, _missionStatusDetailsData.MissionStatusDetails, _missionName, _missionDependTos);
                    _missionStatusDetailsData.Changed = true;
                    //_missionSceneData.MissionStatusDetails = null;
                }
                else
                {
                    // Update chapter status.
                }
            }
        }

        public void MockButtonClicked()
        {
            AllPuzzlePassed();
        }

        private void Start()
        {
            IStepController stepcontroller = _stepControllerGameObject.GetComponent<IStepController>();
            stepcontroller.OnAllStepPassed?.AddListener(AllPuzzlePassed);
        }
    }
}