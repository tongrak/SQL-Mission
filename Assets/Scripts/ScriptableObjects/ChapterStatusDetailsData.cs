using Assets.Scripts.DataPersistence.ChapterStatusDetail;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChapterStatusDetailsData", menuName = "ChapterStatusDetailsData")]
    public class ChapterStatusDetailsData : ScriptableObject
    {
        public bool Changed = false;
        public ChapterStatusDetails ChapterStatusDetails;
    }
}