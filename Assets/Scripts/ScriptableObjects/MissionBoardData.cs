using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MissionBoardData", menuName = "MissionBoardData")]
    public class MissionBoardData : ScriptableObject
    {
        public string[] ChapterFileIndex;
        public string MissionConfigFolderFullPath;
        public bool IsPassed;
    }
}