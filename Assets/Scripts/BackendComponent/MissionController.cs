using Assets.Scripts.DataPersistence.SaveManager;
using Assets.Scripts.DataPersistence.StepController;
using Assets.Scripts.ScriptableObjects;
using Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private GameObject _stepControllerGameObject;
        [SerializeField] private GameObject _gameplayManagerGameObject;
        [SerializeField] private MissionData _missionSceneData;
        [SerializeField] private MissionStatusDetailsData _missionStatusDetailsData;
        [SerializeField] private ChapterStatusDetailsData _chapterStatusDetailsData;
        [SerializeField] private SelectedChapterData _selectedChapterData;

        private MissionType _missionType;
        private int _missionID;
        /// <summary>
        /// Must be like 'Assets/Resources/X/XMissionConfigs/X/ChapterX'
        /// </summary>
        private string _missionFolderFullPath;
        private ISaveManager _saveManager;
        private int _missionConfigIndex;
        private bool _isChapterPassed;
        private List<int> _passedChapterIDs;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missionConfigFolderFullPath">Such as "D:/RootFolder/Assets/Resources/X/X/Chapter1"</param>
        /// <param name="missionID"></param>
        /// <param name="missionDependTos"></param>
        /// <param name="missionType"></param>
        /// <param name="saveManager"></param>
        public void Initiate(ISaveManager saveManager)
        {
            MissionConfig config = _missionSceneData.GetCurrConfig();
            _missionFolderFullPath = _missionSceneData.MissionConfigFolderFullPath;
            _missionID = config.MissionID;
            _missionType = config.MissionType;
            _saveManager = saveManager;
            _missionConfigIndex = _missionSceneData.missionConfigIndex;
            _isChapterPassed = _selectedChapterData.IsPassed;
            _passedChapterIDs = new List<int>();
        }

        public void AllPuzzlePassed()
        {
            if (_missionType != MissionType.Placement)
            {
                if (!_missionStatusDetailsData.IsPassedMission(_missionConfigIndex))
                {
                    _missionStatusDetailsData.MissionStatusDetails = _saveManager.UpdateMissionStatus(_missionFolderFullPath, _missionStatusDetailsData.MissionStatusDetails, _missionID);
                    _missionStatusDetailsData.Changed = true;
                    if (_missionType == MissionType.Final && !_isChapterPassed)
                    {
                        _chapterStatusDetailsData.ChapterStatusDetails = _saveManager.UpdateChapterStatus(_selectedChapterData.ChapterFolderFullPath, _chapterStatusDetailsData.ChapterStatusDetails, _selectedChapterData.ChapterID, true);
                        _chapterStatusDetailsData.Changed = true;
                    }
                }

                // Tell gameplay if "Go" button can be clicked or not
                InitiateEndGameplay();
            }
            else
            {
                SavePlacementResult();
            }
        }

        public void InitiateEndGameplay()
        {
            IGameplayManager gameplayManager = _gameplayManagerGameObject.GetComponent<IGameplayManager>();
            if (_missionSceneData.HaveNextMission())
            {
                gameplayManager.EndGameplay(_missionStatusDetailsData.IsPassedMission(_missionConfigIndex + 1));
            }
            else
            {
                gameplayManager.EndGameplay(false);
            }
        }

        public void ChapterPassed(int passedChapterID)
        {
            _passedChapterIDs.Add(passedChapterID);
        }

        public void SavePlacementResult()
        {
            _passedChapterIDs = _passedChapterIDs.Distinct().ToList();
            for (int i = 0; i < _passedChapterIDs.Count; i++)
            {
                int chapterID = _passedChapterIDs[i];
                if (i == _passedChapterIDs.Count - 1)
                {
                    _chapterStatusDetailsData.ChapterStatusDetails = _saveManager.UpdateChapterStatus(_selectedChapterData.ChapterFolderFullPath, _chapterStatusDetailsData.ChapterStatusDetails, chapterID, true);
                }
                else
                {
                    _chapterStatusDetailsData.ChapterStatusDetails = _saveManager.UpdateChapterStatus(_selectedChapterData.ChapterFolderFullPath, _chapterStatusDetailsData.ChapterStatusDetails, chapterID, false);
                }
            }
        }

        public void GoToNextMissionClicked()
        {
            _missionSceneData.GoToNextMission();
        }

        private void Start()
        {
            IStepController stepcontroller = _stepControllerGameObject.GetComponent<IStepController>();
            stepcontroller.OnAllStepPassed?.AddListener(AllPuzzlePassed);
        }
    }
}