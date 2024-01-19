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
        SceneManager.LoadScene("Mission Scene");
    }

    public void LoadSelectMissionScene()
    {
        SceneManager.LoadScene("Mission Select Scene");
    }
}
