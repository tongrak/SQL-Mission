using Assets.Scripts.Helper;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.MainMenuLayer
{
    public class ContinueController : MonoBehaviour
    {
        [SerializeField] private MainMenuButtonManager _mainMenuButtonManager;

        public void ContinueButtonClicked()
        {
            _mainMenuButtonManager.ContinueButtonClicked();
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