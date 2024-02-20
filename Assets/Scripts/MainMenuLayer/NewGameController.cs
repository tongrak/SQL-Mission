using UnityEngine;
using Assets.Scripts.MainMenuLayer;

public class NewGameController : MonoBehaviour
{
    [SerializeField] private GameObject _confirmationPopUp;
    [SerializeField] private UnityEngine.UI.Button _continueButton;
    [SerializeField] private MainMenuButtonManager _mainMenuButtonManager;

    public void NewGameButtonClicked()
    {
        if (_continueButton.interactable) _confirmationPopUp.SetActive(true);
        else _mainMenuButtonManager.NewGameButtonClicked();
    }
    public void ConfirmNewGame() => _mainMenuButtonManager.NewGameButtonClicked();
    public void CancelNewGame() => _confirmationPopUp.SetActive(false);

    private void Start() => _confirmationPopUp.SetActive(false);
}
