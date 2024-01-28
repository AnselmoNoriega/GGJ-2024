using UnityEngine;
using UnityEngine.UI;

public class Valve : MonoBehaviour
{
    [Header("OBJ Elements")]
    [SerializeField] private Image _blinkingArrow;
    [SerializeField] private GameObject _turnArrows;
    [SerializeField] private GameObject _gauge;
    [SerializeField] private Animator _canisterAnimation;

    private Animator _animation;
    private bool _rotateValveOption;

    private Color _arrowColor;

    private void Awake()
    {
        _animation = _gauge.GetComponent<Animator>();
        _arrowColor = _blinkingArrow.color;
    }

    public void TimerForArowBlink(bool blink)
    {
        if (_rotateValveOption)
        {
            _arrowColor.a = blink == true ? 0.25f : 1.0f;
            _blinkingArrow.color = _arrowColor;
        }
    }

    public void Choices()
    {
        if (!_rotateValveOption && _gauge.transform.rotation.eulerAngles.z == 0)
        {
            ServiceLocator.Get<SoundManager>().PlaySound("ValveTurn");
            ServiceLocator.Get<SoundManager>().PlaySound("TurnSignal");
            _turnArrows.SetActive(true);
            _rotateValveOption = true;
            _animation.Play("ValveTurning");
        }
        else if (_gauge.transform.rotation.eulerAngles.z == 0)
        {
            ServiceLocator.Get<SoundManager>().PlaySound("ValveTurn");
            ServiceLocator.Get<SoundManager>().StopSound("TurnSignal");
            _arrowColor.a = 0.15f;
            _blinkingArrow.color = _arrowColor;
            _turnArrows.SetActive(false);
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
        _arrowColor.a = 0.15f;
        _blinkingArrow.color = _arrowColor;
        _turnArrows.SetActive(false);
        bool choice = _rotateValveOption;
        _rotateValveOption = false;
        ServiceLocator.Get<SoundManager>().StopSound("TurnSignal");
        return choice;
    }

}
