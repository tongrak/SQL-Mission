using Assets.Scripts.DataPersistence.MissionStatusDetail;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionStatusDetailsData", menuName = "MissionStatusDetailsData")]
    public class MissionStatusDetailsData : ScriptableObject
    {
        public bool Changed = false;
        public MissionStatusDetails MissionStatusDetails;

        public bool IsPassedMission(int missionIndex)
        {
            return MissionStatusDetails.MissionStatusDetailList[missionIndex].IsPass;
        }

        public bool IsUnlockedMission(int missionIndex) => MissionStatusDetails.MissionStatusDetailList[missionIndex].IsUnlock;
    }
}