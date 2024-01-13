using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionData", menuName = "MissionSelectedData")]
    public class MissionData: ScriptableObject
    {
        /// <summary>
        /// Path must contain 'Assets' and 'Resources' folder and must be like this 'Assets/X/Resource/X/X/MissionConfigs/ChapterX'
        /// </summary>
        public string missionConfigFolderPathFromAssets;
        /// <summary>
        /// Such as 'Mission1.txt' but insert 'Mission1' only
        /// </summary>
        public string missionFileName;
    }
}
