using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputsControl _inputManager;

    private InputAction _interactPauseMenu;
    //public InputAction name;

    [SerializeField] private GameObject _pauseMenu;

    private void Awake()
    {
        _inputManager = new InputsControl();
        _interactPauseMenu = _inputManager.Player.PauseMenu;
    }

    private void OnEnable()
    {
        _interactPauseMenu.Enable();
        _interactPauseMenu.performed += ButtonPauseMenu;
    }

    private void OnDisable()
    {
        _interactPauseMenu.Disable();
        _interactPauseMenu.performed -= ButtonPauseMenu;
    }

    private void MethodName(InputAction.CallbackContext input)
    {

    }

    private void ButtonPauseMenu(InputAction.CallbackContext input)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeInHierarchy);
        Time.timeScale = _pauseMenu.activeInHierarchy == false ? 1 : 0;
    }
}