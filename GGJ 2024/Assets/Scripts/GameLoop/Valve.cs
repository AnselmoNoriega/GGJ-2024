using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    [SerializeField] private Image[] _selectionImage; 
    private bool _rotateValveOption;

    public void Choices(bool choice)
    {
        _rotateValveOption = choice;
        if(choice)
        {
            _selectionImage[0].color = Color.white;
            _selectionImage[1].color = Color.red;
        }
        else
        {
            _selectionImage[1].color = Color.white;
            _selectionImage[0].color = Color.red;
        }
    }

    public void Rotate()
    {
        Vector3 currentRot = transform.rotation.eulerAngles;
        float rotationAngle = currentRot.y + 180.0f;
        transform.rotation = Quaternion.Euler(currentRot.x, rotationAngle, currentRot.z);
    }

    public bool ReturnChoice()
    {
        foreach(var item in _selectionImage)
        {
            item.color = Color.white;
        }

        bool choice = _rotateValveOption;
        _rotateValveOption = false;
        return choice;
    }

}
