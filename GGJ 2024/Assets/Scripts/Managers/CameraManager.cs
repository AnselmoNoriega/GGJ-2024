using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Vector2 _maxDistance;
    [SerializeField] private float _sensitibity = 2.0f;

    [SerializeField] private Vector2 _screenMiddlePoint;

    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;

    private void Awake()
    {
        _screenMiddlePoint = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    private void Update()
    {
        Vector2 disFromCenter = (Vector2)Input.mousePosition - _screenMiddlePoint;

        if( _maxDistance.x <= Mathf.Abs(disFromCenter.x))
        {
            Vector3 currentRot = transform.rotation.eulerAngles;
            float rotationAngleX = Mathf.Clamp(currentRot.y + disFromCenter.normalized.x *_sensitibity, -45, 45);
            transform.rotation = Quaternion.Euler(currentRot.x, rotationAngleX, currentRot.z);
        }

        //if( _maxDistance.y <= Mathf.Abs(disFromCenter.y))
        //{
        //    Vector3 currentRot = transform.rotation.eulerAngles;
        //    float rotationAngleY = currentRot.x - disFromCenter.normalized.y *_sensitibity;
        //    transform.rotation = Quaternion.Euler(rotationAngleY, currentRot.y, currentRot.z);
        //}
    }
}
