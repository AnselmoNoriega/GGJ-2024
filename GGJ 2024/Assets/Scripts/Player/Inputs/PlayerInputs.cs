using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputsControl _inputManager;

    //public InputAction name;

    private void Awake()
    {
        _inputManager = new InputsControl();
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
}