using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionData", menuName = "MissionSelectedData")]
    public class MissionData: ScriptableObject
    {
        public string missionConfigFolderPath; // Path must be after 'Resources' folder and must be like this 'MissionConfigs/ChapterX'
        public string missionFileName; // Such as 'Mission1.txt' but insert 'Mission1' only
    }
}
