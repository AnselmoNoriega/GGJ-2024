using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : MonoBehaviour
{
    [Header("Roulette Info")]
    [SerializeField] private Rigidbody _rouletteRb;

    [Header("Pointer Info")]
    [SerializeField] float _pointerDistance;
    private Ray _rouletteRay;
    private RaycastHit _rouletteHit;

    [Header("Force Info")]
    [SerializeField] Slider _forceSlider;
    [SerializeField] float _maxForce;
    [SerializeField] float _forceCountSpeed;
    private Vector3 _force = Vector3.zero;
    private float _forceCounter = 0;

    private bool _isChargingSpin = false;
    private bool _canSpin = true;

    private void Start()
    {
        _rouletteRay = new Ray(transform.position, Vector3.down);
        _forceSlider.maxValue = _maxForce;
        _forceSlider.value = 0.0f;
    }

    public void TurnRoulette(Vector3 torqueForce)
    {
        _rouletteRb.AddTorque(torqueForce);
    }

    private void Update()
    {
        if (_isChargingSpin && _canSpin)
        {
            ChargeSpin();
        }

        if(!_canSpin && _rouletteRb.angularVelocity == Vector3.zero)
        {
            CheckSpinResult();
            _canSpin = true;
        }
    }

    private void ChargeSpin()
    {
        _forceCounter += Time.deltaTime * _forceCountSpeed;
        _forceSlider.value = _forceCounter;
        Debug.Log(_forceCounter);

        if (_forceCounter >= _maxForce || _forceCounter <= 0.0f)
        {
            _forceCountSpeed *= (-1);
        }
    }

    private void CheckSpinResult()
    {
        if(Physics.Raycast(_rouletteRay, out _rouletteHit, _pointerDistance))
        {
            Debug.Log(_rouletteHit.collider.name);
        }
    }

    public void StartChargingSpin()
    {
        _isChargingSpin = true;
    }

    public void Start2Spin()
    {
        _force.z = _forceCounter;
        TurnRoulette(_force);
        _forceCounter = 0.0f;
        _forceSlider.value = _forceCounter;
        _isChargingSpin = false;
        _canSpin = false;
    }

    public void StopRoulette()
    {
        _rouletteRb.angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Vector3.down * _pointerDistance);
    }
}
