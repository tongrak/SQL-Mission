using Assets.Scripts.Helper;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NewGameController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingFacade;

    private FileSystemWatcher _fileWatcher;
    private string _rootPath;
    private string _statusFile;
    private int _totalStatusFile;
    private int _totalDeletedStatusFile;
    private bool _deleteCompleted;

    private void _SetFields()
    {
        _rootPath = Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder);
        _statusFile = EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType;
        _totalStatusFile = 0;
        _totalDeletedStatusFile = 0;
        _deleteCompleted = false;
    }

    public void NewGameButtonClicked()
    {
        _loadingFacade.SetActive(true);
        if (_fileWatcher != null)
        {
            _DeleteAllStatusFile();
            // Will change scene after delete all status file.
        }
        else
        {
            ScenesManager.Instance.LoadSelectChapterScene();
        }
    }

    private void _DeleteAllStatusFile()
    {
        string[] statusfileList = Directory.GetFiles(_rootPath, _statusFile + "*", SearchOption.AllDirectories);
        _totalStatusFile = statusfileList.Length;

        foreach (string statusfile in statusfileList)
        {
            File.Delete(statusfile);
        }
    }

    private void _StatusFileDeleted(object sender, FileSystemEventArgs e)
    {
        _totalDeletedStatusFile++;
        Debug.LogWarning("Deleted file: " + e.FullPath);

        if (_totalDeletedStatusFile == _totalStatusFile)
        {
            _deleteCompleted = true;
        }
    }

    private void _InitiateFileWatcher()
    {
        _fileWatcher = new FileSystemWatcher(_rootPath, _statusFile + "*");
        _fileWatcher.NotifyFilter = NotifyFilters.CreationTime
         | NotifyFilters.LastWrite
         | NotifyFilters.Size;

        _fileWatcher.Deleted += _StatusFileDeleted;

        _fileWatcher.IncludeSubdirectories = true;
        _fileWatcher.EnableRaisingEvents = true;
    }

    private void Start()
    {
        _SetFields();

        if (File.Exists(Path.Combine(Application.dataPath, "Resources", EnvironmentData.Instance.ChapterConfigRootFolder, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType)))
        {
            _InitiateFileWatcher();
        }
    }

    private void Update()
    {
        if (_deleteCompleted)
        {
            _deleteCompleted = false;
            ScenesManager.Instance.LoadSelectChapterScene();
        }
    }

    private void OnDestroy()
    {
        if (_fileWatcher != null)
        {
            _fileWatcher.Deleted -= _StatusFileDeleted;
        }
    }
}
