using UnityEngine;

namespace Assets.Scripts.MainMenuLayer
{
    public class QuitController : MonoBehaviour
    {
        public void QuitButtonClicked()
        {
            Application.Quit();
        }
    }
}