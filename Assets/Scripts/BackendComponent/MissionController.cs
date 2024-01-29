using Assets.Scripts.DataPersistence.SaveManager;
using Assets.Scripts.DataPersistence.StepController;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.DataPersistence
{
    public class MissionController : MonoBehaviour
    {
        [SerializeField] private GameObject _stepControllerGameObject;
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
        private bool _isPassed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="missionConfigFolderFullPath">Such as "D:/RootFolder/Assets/Resources/X/X/Chapter1"</param>
        /// <param name="missionID"></param>
        /// <param name="missionDependTos"></param>
        /// <param name="missionType"></param>
        /// <param name="saveManager"></param>
        public void Initiate(string missionConfigFolderFullPath, int missionID, MissionType missionType, ISaveManager saveManager, bool isPassed)
        {
            _missionFolderFullPath = missionConfigFolderFullPath;
            _missionID = missionID;
            _missionType = missionType;
            _saveManager = saveManager;
            _isPassed = isPassed;
        }

        public void AllPuzzlePassed()
        {
            if (!_isPassed)
            {
                _missionStatusDetailsData.MissionStatusDetails = _saveManager.UpdateMissionStatus(_missionFolderFullPath, _missionStatusDetailsData.MissionStatusDetails, _missionID);
                _missionStatusDetailsData.Changed = true;
                if (_missionType == MissionType.Final)
                {
                    _chapterStatusDetailsData.ChapterStatusDetails = _saveManager.UpdateChapterStatus(_selectedChapterData.ChapterFolderFullPath, _chapterStatusDetailsData.ChapterStatusDetails, _selectedChapterData.ChapterID);
                    _chapterStatusDetailsData.Changed = true;
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