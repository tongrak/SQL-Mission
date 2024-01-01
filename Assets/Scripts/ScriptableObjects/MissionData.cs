using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionData", menuName = "MissionSelectedData")]
    public class MissionData: ScriptableObject
    {
        public string missionConfigPath;
    }
}
