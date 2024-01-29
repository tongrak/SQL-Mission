using Assets.Scripts.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.BackendComponent
{
    /// <summary>
    /// Use for keep SO data that don't use on this scene but want to carry to other scene.
    /// </summary>
    public class MissionPersistenceSO : MonoBehaviour
    {
        [SerializeField] private MissionBoardData _missionBoardData;
    }
}