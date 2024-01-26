using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputsControl _inputManager;

    private InputAction _pauseMenu;
    //public InputAction name;

    private void Awake()
    {
        _inputManager = new InputsControl();
        _pauseMenu = _inputManager.Player.PauseMenu;
        _pauseMenu.Enable();
        _pauseMenu.performed += ButtonPauseMenu; 
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

    }
}