using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    private bool _rotateValveOption;

    public void Choices()
    {
        _rotateValveOption = true;
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
