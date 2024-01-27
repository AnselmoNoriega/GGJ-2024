using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private float _timePerRound = 1.0f;
    [SerializeField] private Slider _timerUI;
    private float _timer;

    [SerializeField] private Valve[] valves;

    [SerializeField] private GameObject _PlayerPointer;
    [SerializeField] private GameObject _AIPointer;
    [SerializeField] private GameObject _panelEnding;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private int _AI_Health = 5;

    private bool _loaded = false;
    private bool _gameOver = false;
    private bool _gameOnGoing = true;

    public void Load()
    {
        _timer = _timePerRound;
        _loaded = true;
        ServiceLocator.Get<SoundManager>().PlayMainSound("Start");
        _timerUI.maxValue = _timePerRound;
    }

    private void Update()
    {
        if (!_loaded || _gameOver || !_gameOnGoing)
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
            StartCoroutine(GetOptions());
        }
    }

    private IEnumerator GetOptions()
    {
        bool[] playerOptions = { valves[0].ReturnChoice(), valves[1].ReturnChoice() };

        _gameOnGoing = false;

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

        yield return new WaitForSeconds(3);

        if (valves[0].transform.rotation.y == valves[1].transform.rotation.y)
        {
            if (Mathf.RoundToInt(valves[0].transform.rotation.eulerAngles.y) == 0)
            {
                int health = --ServiceLocator.Get<Player>().Lives;
                _PlayerPointer.transform.localRotation = Quaternion.Euler(_PlayerPointer.transform.localRotation.eulerAngles.x, _PlayerPointer.transform.localRotation.eulerAngles.y, _PlayerPointer.transform.localRotation.eulerAngles.z - 30f);
                StartCoroutine(ServiceLocator.Get<ParticleManager>().ActivateGasEffect(2f));

                if (health <= 0)
                {
                    FinishGame("Ai");
                }

                CheckMusic(health);
            }
            else
            {
                --_AI_Health;
                _AIPointer.transform.localRotation = Quaternion.Euler(_AIPointer.transform.localRotation.eulerAngles.x, _AIPointer.transform.localRotation.eulerAngles.y, _AIPointer.transform.localRotation.eulerAngles.z - 30f);

                if (_AI_Health <= 0)
                {
                    FinishGame("Player");
                }
            }
        }

        _timer = _timePerRound;
        _gameOnGoing = true;
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
        _panelEnding.SetActive(true);
        if (winner == "Player")
        {
            _gameOverText.text = "You Escaped!";
            _gameOverText.color = Color.white;
        }
        else
        {
            _gameOverText.text = "Game Over";
            _gameOverText.color = Color.red;
        }
    }
}
