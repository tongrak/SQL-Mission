using Assets.Scripts.DataPersistence.MissionStatusDetail;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionStatusDetailsData", menuName = "MissionStatusDetailsData")]
    public class MissionStatusDetailsData : ScriptableObject
    {
        public bool Changed = false;
        public MissionUnlockDetails MissionStatusDetails;

        public bool IsPassedMission(int missionIndex)
        {
            return MissionStatusDetails.MissionUnlockDetailList[missionIndex].IsPass;
        }

        public bool IsUnlockedMission(int missionIndex) => MissionStatusDetails.MissionUnlockDetailList[missionIndex].IsUnlock;
    }
}