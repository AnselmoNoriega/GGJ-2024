using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    [SerializeField] private GameObject _gauge;
    [SerializeField] private Animator _canisterAnimation;
    private Animator _animation;
    private bool _rotateValveOption;

    private void Awake()
    {
        _animation = _gauge.GetComponent<Animator>();
    }

    public void Choices()
    {
        if (!_rotateValveOption && _gauge.transform.rotation.eulerAngles.z == 0)
        {
            _rotateValveOption = true;
            _animation.Play("ValveTurning");
        }
        else if(_gauge.transform.rotation.eulerAngles.z == 0)
        {
            _rotateValveOption = false;
            _animation.Play("ValveTuningBack");
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
