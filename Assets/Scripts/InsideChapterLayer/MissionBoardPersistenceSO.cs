using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.InsideChapterLayer
{
    /// <summary>
    /// Use for keep SO data that don't use on this scene but want to carry to other scene.
    /// </summary>
    public class MissionBoardPersistenceSO : MonoBehaviour
    {
        [SerializeField] private ChapterStatusDetailsData _chapterStatusData;
        [SerializeField] private SelectedChapterData _selectedChapterData;
    }
}