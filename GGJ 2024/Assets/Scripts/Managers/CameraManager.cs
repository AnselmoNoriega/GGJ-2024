using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float _maxXAngle;
    [SerializeField] private float _maxYAngle;
    [SerializeField] private float _sensitivity = 0.1f;

    private Vector2 _screenMiddlePoint;

    private void Awake()
    {
        _screenMiddlePoint = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    private void Update()
    {
        if (Time.timeScale > 0)
        {
            Vector2 disFromCenter = (Vector2)Input.mousePosition - _screenMiddlePoint;

            Vector3 currentRot = transform.rotation.eulerAngles;
            float rotationAngleX = Mathf.Clamp(-disFromCenter.y * _sensitivity, -_maxXAngle, _maxXAngle);
            float rotationAngleY = Mathf.Clamp(disFromCenter.x * _sensitivity, -_maxYAngle, _maxYAngle);
            transform.eulerAngles = new Vector3(rotationAngleX, rotationAngleY, currentRot.z);
        }
    }
}
