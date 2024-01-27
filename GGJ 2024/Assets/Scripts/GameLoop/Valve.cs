using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    [SerializeField] private Animator _animation;
    [SerializeField] private Animator _canisterAnimation;
    private bool _rotateValveOption;

    public void Choices()
    {
        if (!_rotateValveOption)
        {
            _rotateValveOption = true;
            _animation.Play("ValveTurning");
        }
    }

    public void Rotate()
    {
        _canisterAnimation.SetTrigger("Rotate");
    }

    public bool ReturnChoice()
    {
        bool choice = _rotateValveOption;
        _rotateValveOption = false;
        return choice;
    }

}
