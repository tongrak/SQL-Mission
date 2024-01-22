using Assets.Scripts.DataPersistence;
using System.IO;
using UnityEngine;

public class SaveAndBackToMB : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MissionController _missionController;
    [SerializeField] private ScenesManager _scenesManager;
    private FileSystemWatcher _watcher;
    private bool _changed = false;

    public void ButtonClicked()
    {
        _missionController.AllPuzzlePassed();
    }

    public void FileChanged(object sender, FileSystemEventArgs e)
    {
        _changed = true;
    }

    public void Initiate(FileSystemWatcher watcher)
    {
        _watcher = watcher;
        _watcher.Changed += FileChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if( _changed )
        {
            Debug.Log("Mission status changed;");
            _scenesManager.LoadSelectMissionScene();
            _changed = false;
        }
    }

    private void OnDisable()
    {
        _watcher.Changed -= FileChanged;
    }
}
