using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionSceneBoardData", menuName = "MissionSceneBoardData")]
    public class MissionBoardData : ScriptableObject
    {
        public string[] ChapterFileIndex;
        public string MissionConfigFolderFullPath;
    }
}