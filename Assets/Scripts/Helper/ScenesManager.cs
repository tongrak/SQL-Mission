using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadMissionScene()
    {
        SceneManager.LoadScene("Gameplay Scene");
    }

    public void LoadSelectMissionScene()
    {
        SceneManager.LoadScene("Mission Select Scene");
    }

    public void LoadSelectChapterScene()
    {
        SceneManager.LoadScene("Chapter Scene");
    }
}
