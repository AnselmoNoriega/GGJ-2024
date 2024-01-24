using UnityEngine;
using TMPro;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private float _timePerRound = 1.0f;
    [SerializeField] private TextMeshProUGUI _timerUI;
    private float _timer;

    [SerializeField] private Valve[] valves;

    [SerializeField] private TextMeshProUGUI _PlayerHealthUI;
    [SerializeField] private TextMeshProUGUI _AIHealthUI;
    [SerializeField] private int _AI_Health = 5;

    private bool _loaded = false;
    private bool _gameOver = false;

    public void Load()
    {
        _timer = _timePerRound;
        _PlayerHealthUI.text = "Player Health: " + ServiceLocator.Get<Player>().Lives;
        _AIHealthUI.text = "AI Health: " + _AI_Health;
        _loaded = true;
    }

    private void Update()
    {
        if(!_loaded || _gameOver)
        {
            return;
        }

        if (_timer >= 0.0f)
        {
            _timer -= Time.deltaTime;
            _timerUI.text = Mathf.FloorToInt(_timer).ToString();
        }
        else
        {
            GetOptions();
        }
    }

    private void GetOptions()
    {
        bool[] playerOptions = { valves[0].ReturnChoice(), valves[1].ReturnChoice()};

        _timer = _timePerRound;
        for(int i = 0; i < 2; ++i)
        {
            if(playerOptions[i])
            {
                valves[i].Rotate();
            }

            if(Random.Range(1, 101) <= 50)
            {
                valves[i].Rotate();
            }
        }

        if (valves[0].transform.rotation.y == valves[1].transform.rotation.y)
        {
            if(Mathf.RoundToInt(valves[0].transform.rotation.eulerAngles.y) == 0)
            {
                int health = --ServiceLocator.Get<Player>().Lives;
                _PlayerHealthUI.text = "Player Health: " + health;

                if(health <= 0)
                {
                    FinishGame("Ai");
                }
            }
            else
            {
                --_AI_Health;
                _AIHealthUI.text = "AI Health: " + _AI_Health;

                if (_AI_Health <= 0)
                {
                    FinishGame("Player");
                }
            }
        }
    }

    public void EndTime()
    {
        _timer = 0.0f;
    }

    private void FinishGame(string winner)
    {
        _gameOver = true;
        _AIHealthUI.text = winner + " is the winner";
        _PlayerHealthUI.text = winner + " is the winner";
        _timerUI.text = "";
    }
}
