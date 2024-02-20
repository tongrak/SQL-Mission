using UnityEngine;
using Assets.Scripts.MainMenuLayer;

public class NewGameController : MonoBehaviour
{
    [SerializeField] private GameObject _confirmationPopUp;
    [SerializeField] private MainMenuButtonManager _mainMenuButtonManager;

    public void NewGameButtonClicked() => _confirmationPopUp.SetActive(true);
    public void ConfirmNewGame() => _mainMenuButtonManager.NewGameButtonClicked();
    public void CancelNewGame() => _confirmationPopUp.SetActive(false);

    private void Start() => _confirmationPopUp.SetActive(false);
}
