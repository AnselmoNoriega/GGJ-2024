using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputsControl _inputManager;

    public InputAction _spinWheel;
    public InputAction _stopnWheel;

    private void Awake()
    {
        _inputManager = new InputsControl();
    }

    private void OnEnable()
    {
        _spinWheel = _inputManager.Player.SpinWheel;
        _spinWheel.Enable();
        _spinWheel.performed += StartChargingSpin;
        _spinWheel.canceled += StartWheelSpinning;

        _spinWheel = _inputManager.Player.StopWheel;
        _stopnWheel.Enable();
        _stopnWheel.performed += StopWheel;
    }

    private void OnDisable()
    {
        _spinWheel.Disable();
        _spinWheel.performed -= StartChargingSpin;
        _spinWheel.canceled -= StartWheelSpinning;

        _stopnWheel.Enable();
        _stopnWheel.performed -= StopWheel;
    }

    private void StartChargingSpin(InputAction.CallbackContext input)
    {
        ServiceLocator.Get<GameObjManager>().Roulette.StartChargingSpin();
    }

    private void StartWheelSpinning(InputAction.CallbackContext input)
    {
        ServiceLocator.Get<GameObjManager>().Roulette.Start2Spin();
    }

    private void StopWheel(InputAction.CallbackContext input)
    {
        ServiceLocator.Get<GameObjManager>().Roulette.StopRoulette();
    }
}