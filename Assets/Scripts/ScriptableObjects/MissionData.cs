using Assets.Scripts.DataPersistence.MissionStatusDetail;
using Assets.Scripts.DataPersistence.StepController;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionSceneData", menuName = "MissionSceneData")]
    public class MissionData: ScriptableObject
    {
        /// <summary>
        /// Path must contain 'Assets' and 'Resources' folder and must be like this 'Assets/X/Resource/X/X/MissionConfigs/ChapterX'
        /// </summary>
        public string MissionConfigFolderFullPath;

        public MissionConfig[] missionConfigs;

        public int missionConfigIndex;

        public MissionConfig GetCurrConfig()
        {
            return missionConfigs[missionConfigIndex];
        }

        public bool HaveNextMission()
        {
            return missionConfigIndex + 1 < missionConfigs.Length;
        }

        public void GoToNextMission()
        {
            missionConfigIndex++;
        }
    }
}
