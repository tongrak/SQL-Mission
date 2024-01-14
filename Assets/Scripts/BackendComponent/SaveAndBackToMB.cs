using Assets.Scripts.BackendComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAndBackToMB : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MissionController _missionController;
    [SerializeField] private ScenesManager _scenesManager;

    public void ButtonClicked()
    {
        _missionController.AllPuzzlePassed();
        _scenesManager.LoadSelectMissionScene();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
