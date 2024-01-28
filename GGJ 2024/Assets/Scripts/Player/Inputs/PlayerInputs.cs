using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputsControl _inputManager;

    private InputAction _interactPauseMenu;
    private InputAction _continueText;

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameEndScreen;

    private void Awake()
    {
        _inputManager = new InputsControl();
        _interactPauseMenu = _inputManager.Player.PauseMenu;
        _continueText = _inputManager.Player.ContinueText;
    }

    public void Load()
    {
        _interactPauseMenu.Enable();
        _interactPauseMenu.performed += ButtonPauseMenu;

        _continueText.Enable();
        _continueText.performed += ServiceLocator.Get<TextManager>().OnClick;
    }

    private void OnDisable()
    {
        _interactPauseMenu.Disable();
        _interactPauseMenu.performed -= ButtonPauseMenu;

        _continueText.Disable();
        _continueText.performed -= ServiceLocator.Get<TextManager>().OnClick;
    }

    private void ButtonPauseMenu(InputAction.CallbackContext input)
    {
        if (_gameEndScreen.activeInHierarchy == false)
        {
            _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
            Time.timeScale = _pauseMenu.activeInHierarchy == false ? 1 : 0;
            ServiceLocator.Get<CursorClass>().TogglePauseMenu();
        }
        else
        {
            ServiceLocator.Get<UIManager>().ButtonSceneLoader("MainMenu");
        }
        
    }
}