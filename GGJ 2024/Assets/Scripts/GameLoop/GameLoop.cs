using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;

public class GameLoop : MonoBehaviour
{
    [Header("Loop Info")]
    [SerializeField] private int _AI_Health = 5;
    [SerializeField] private float _timePerRound = 1.0f;
    private float _timer;

    [Space, Header("Object References")]
    [SerializeField] private Valve[] valves;
    [SerializeField] private GameObject _PlayerPointer;
    [SerializeField] private GameObject _AIPointer;
    [SerializeField] private GameObject _panelEnding;

    [Space, Header("UI References")]
    [SerializeField] private Slider _timerUI;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    [Space, Header("Info for Valves")]
    [SerializeField] private float _blinkingTime = 0.3f;
    private float _blinkingArrowTimer;
    private bool _shouldBlink = true;

    [Space, Header("Stories")]
    [SerializeField] private List<TextAsset> _stories;

    private bool _loaded = false;
    private bool _gameOver = false;
    private bool _gameOnGoing = true;

    public void Load()
    {
        _timer = _timePerRound;
        _loaded = true;
        ServiceLocator.Get<SoundManager>().PlayMainSound("Start");
        _timerUI.maxValue = _timePerRound;
        _blinkingArrowTimer = _blinkingTime;
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

        BlinkingTimer();
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
        }
        yield return new WaitForSeconds(2);

        for (int i = 0; i < 2; ++i)
        {
            if (Random.Range(1, 101) <= 50)
            {
                valves[i].Rotate();
            }
        }
        yield return new WaitForSeconds(2.5f);

        if (valves[0].transform.rotation.y == valves[1].transform.rotation.y)
        {
            if (Mathf.RoundToInt(valves[0].transform.rotation.eulerAngles.y) == 0)
            {
                int health = --ServiceLocator.Get<Player>().Lives;
                var playerHealthsAngle = _PlayerPointer.transform.localRotation.eulerAngles;
                _PlayerPointer.transform.localRotation = Quaternion.Euler(playerHealthsAngle.x, playerHealthsAngle.y, playerHealthsAngle.z - 30f);
                StartCoroutine(ServiceLocator.Get<ParticleManager>().ActivateGasEffect(2f));
                ServiceLocator.Get<VisualEffects>().SetBlur(health);

                if (health <= 0)
                {
                    FinishGame("Ai");
                }

                CheckMusic(health);

            }
            else
            {
                --_AI_Health;
                var aiHealthAngle = _AIPointer.transform.localRotation.eulerAngles;
                _AIPointer.transform.localRotation = Quaternion.Euler(aiHealthAngle.x, aiHealthAngle.y, aiHealthAngle.z - 30f);

                if (_AI_Health <= 0)
                {
                    FinishGame("Player");
                }
            }
        }

        ServiceLocator.Get<UIManager>().ButtonSetActive(true);
        _timer = _timePerRound;

        ServiceLocator.Get<TextManager>().EnableStory(null);
    }

    private void BlinkingTimer()
    {
        if (_blinkingArrowTimer <= 0.0f)
        {
            _blinkingArrowTimer = _blinkingTime;
            for (int i = 0; i < 2; ++i)
            {
                valves[i].TimerForArowBlink(_shouldBlink);
            }
            _shouldBlink = !_shouldBlink;
        }
        else
        {
            _blinkingArrowTimer -= Time.deltaTime;
        }
    }

    public void EndTime()
    {
        if (_timer > 0.0f && _gameOnGoing)
        {
            _timer = 0.0f;
        }
    }

    public void ContinueGame()
    {
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

    private void FinishGame(string winner)
    {
        _gameOver = true;
        _panelEnding.SetActive(true);
        if (winner == "Player")
        {
            _gameOverText.text = "You Escaped!";
            _gameOverText.color = Color.white;
            ServiceLocator.Get<SoundManager>().PlayMainSound("Win");
        }
        else
        {
            _gameOverText.text = "Game Over";
            _gameOverText.color = Color.red;
            ServiceLocator.Get<SoundManager>().PlayMainSound("Lose");
        }
    }
}
