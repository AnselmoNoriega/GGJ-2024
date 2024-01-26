using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    enum ValvesLookingDirection
    {
        TwoTowardsEnemy,
        OneTowardsPlayer,
        TwoTowardsPlayer,
    }

    [SerializeField] private float _maxGasAmt = 1.0f;
    [SerializeField] private float _timePerRound = 1.0f;
    [SerializeField] private Slider _timerUI;
    [SerializeField] private Slider[] _healthSliders;
    private ValvesLookingDirection _pipesLooking2Player = ValvesLookingDirection.OneTowardsPlayer;
    private float _timer;

    [SerializeField] private Valve[] valves;

    [SerializeField] private GameObject _PlayerPointer;
    [SerializeField] private GameObject _AIPointer;
    [SerializeField] private int _AI_Health = 5;
    [SerializeField] private GameObject _pauepanel;

    private bool _loaded = false;
    private bool _gameOver = false;

    public void Load()
    {
        _timer = _timePerRound;
        _loaded = true;
        ServiceLocator.Get<SoundManager>().PlayMainSound("Start");
        _timerUI.maxValue = _timePerRound;

        foreach (var healthSlider in _healthSliders)
        {
            healthSlider.maxValue = _maxGasAmt;
            healthSlider.value = _maxGasAmt;
        }
    }

    private void Update()
    {
        if (!_loaded || _gameOver)
        {
            return;
        }

        if (_timer >= 0.0f)
        {
            _timer -= Time.deltaTime;
            _timerUI.value = _timer;
        }
        else
        {
            GetOptions();
        }

        _healthSliders[0].value -= Time.deltaTime * (int)_pipesLooking2Player;
        float val2 = 2 - (int)_pipesLooking2Player;
        float val = Time.deltaTime * val2;
        _healthSliders[1].value -= val;
        if (_healthSliders[0].value <= 0)
        {
            FinishGame("Ai");
        }
        else if (_healthSliders[1].value <= 0)
        {
            FinishGame("Player");
        }
    }

    private void GetOptions()
    {
        bool[] playerOptions = { valves[0].ReturnChoice(), valves[1].ReturnChoice() };

        _timer = _timePerRound;
        for (int i = 0; i < 2; ++i)
        {
            if (playerOptions[i])
            {
                valves[i].Rotate();
            }

            if (Random.Range(1, 101) <= 50)
            {
                valves[i].Rotate();
            }
        }

        if (Mathf.RoundToInt(valves[0].transform.rotation.eulerAngles.y) == 0 && Mathf.RoundToInt(valves[1].transform.rotation.eulerAngles.y) == 0)
        {
            _pipesLooking2Player = ValvesLookingDirection.TwoTowardsPlayer;
        }
        else if(Mathf.RoundToInt(valves[0].transform.rotation.eulerAngles.y) == 0 || Mathf.RoundToInt(valves[1].transform.rotation.eulerAngles.y) == 0)
        {
            _pipesLooking2Player = ValvesLookingDirection.OneTowardsPlayer;
        }
        else
        {
            _pipesLooking2Player = ValvesLookingDirection.TwoTowardsEnemy;
        }
    }

    private void CheckMusic(int playerHealth)
    {
        switch (playerHealth)
        {
            case 4:
                {
                    ServiceLocator.Get<SoundManager>().PlayMainSound("Climax");
                }
                return;
            case 2:
                {
                    ServiceLocator.Get<SoundManager>().PlayMainSound("Ending");
                }
                return;
        }
    }

    public void EndTime()
    {
        _timer = 0.0f;
    }

    private void FinishGame(string winner)
    {
        _gameOver = true;
        _pauepanel.SetActive(true);
    }
}
