using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour
{
    private bool _rotateValveOption;

    public void Choices(bool choice)
    {
        _rotateValveOption = choice;
    }

    public void Rotate()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        float rotationAngle = currentRot.y + 180.0f;
        transform.rotation = Quaternion.Euler(currentRot.x, rotationAngle, currentRot.z);
    }

    public bool ReturnChoice()
    {
        bool choice = _rotateValveOption;
        _rotateValveOption = false;
        return choice;
    }

}
