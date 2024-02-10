using UnityEngine;
using Assets.Scripts.MainMenuLayer;

public class NewGameController : MonoBehaviour
{
    [SerializeField] private MainMenuButtonManager _mainMenuButtonManager;

    public void NewGameButtonClicked()
    {
        _mainMenuButtonManager.NewGameButtonClicked();
    }
}
