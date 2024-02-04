using Assets.Scripts.Helper;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuLayer
{
    public class ContinueController : MonoBehaviour
    {
        public void ContinueButtonClicked()
        {
            ScenesManager.Instance.LoadSelectChapterScene();
        }

        public void Start()
        {
            if (File.Exists(Path.Combine(Application.dataPath, EnvironmentData.Instance.ResourcesFolder, EnvironmentData.Instance.ChapterConfigRootFolder, EnvironmentData.Instance.StatusFileName + EnvironmentData.Instance.ConfigFileType)))
            {
                gameObject.GetComponent<Button>().interactable = true;
            }
        }
    }
}