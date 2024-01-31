using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SelectedChapterData", menuName = "SelectedChapterData")]
    public class SelectedChapterData : ScriptableObject
    {
        public string ChapterFolderFullPath;
        public int ChapterID;
    }
}