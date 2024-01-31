using Assets.Scripts.Helper;
using System.IO;
using UnityEngine;

public class NewGameController : MonoBehaviour
{
    public void NewGameButtonClicked()
    {
        string rootPath = Path.Combine(Application.dataPath, "Resources");
        string[] statusfileList = Directory.GetFiles(rootPath, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType + "*", SearchOption.AllDirectories);

        foreach (string statusfile in statusfileList)
        {
            File.Delete(statusfile);
            Debug.Log("Deleted file: " + statusfile);
        }
    }

    private void Start()
    {
        // Delete all status file
    }
}
