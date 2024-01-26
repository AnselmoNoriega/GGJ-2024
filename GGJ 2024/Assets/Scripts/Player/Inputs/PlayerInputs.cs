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
        _interactPauseMenu.Enable();
        _interactPauseMenu.performed += ButtonPauseMenu; 
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {

    }

    private void MethodName(InputAction.CallbackContext input)
    {
        
    } 

    private void ButtonPauseMenu(InputAction.CallbackContext input)
    {
        if (_pauseMenu.activeInHierarchy == true)
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}