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
    [SerializeField] private Valve[] _valves;
    [SerializeField] private GameObject _playerPointer;
    [SerializeField] private GameObject _AIPointer;
    [SerializeField] private GameObject _escapeEnding;
    [SerializeField] private GameObject _gameOverEnding;

    [Space, Header("UI References")]
    [SerializeField] private Slider _timerUI;
    [SerializeField] private TextMeshProUGUI _turnText;

    [Space, Header("Info for Valves")]
    [SerializeField] private float _blinkingTime = 0.3f;
    private float _blinkingArrowTimer;
    private bool _shouldBlink = true;
    private bool[] valveValues = new bool[] { false, false }; // True faces the player, false faces the prisoner

    [Space, Header("Stories")]
    [SerializeField] private List<TextAsset> _storiesMidHealth;
    [SerializeField] private List<TextAsset> _storiesLowHealth;

    [SerializeField] private List<TextAsset> _storiesOneHealth;
    [SerializeField] private List<TextAsset> _storiesTwoHealth;
    [SerializeField] private List<TextAsset> _storiesThreeHealth;
    [SerializeField] private List<TextAsset> _storiesFourHealth;

    [SerializeField] private List<TextAsset> _notTurningHint;
    [SerializeField] private List<TextAsset> _turningHint;
    [SerializeField] private List<TextAsset> _playerGassed;

    // PRISONER LOGIC
    private Prisoner prisoner;
    private bool[] prisonerOptions;

private bool _playerGotGassed = false;

    private bool _loaded = false;
    private bool _gameOver = false;
    private bool _gameOnGoing = true;

    public void Load()
    {
        _timer = _timePerRound;
        _loaded = true;
        _timerUI.maxValue = _timePerRound;
        _blinkingArrowTimer = _blinkingTime;
        valveValues = new bool[] { true, false };
        prisoner = new Prisoner();
        prisonerOptions = prisoner.GetNextMove(true, false);
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
        bool[] playerOptions = { _valves[0].ReturnChoice(), _valves[1].ReturnChoice() };
        prisoner.LogPlayerMove(playerOptions[0], playerOptions[1]);

        _gameOnGoing = false;
        ServiceLocator.Get<CursorClass>().SetPipesTurningToTrue();
        ServiceLocator.Get<CursorClass>().CursorFrustratedWhilePipesTurning();

        _turnText.gameObject.SetActive(true);
        _turnText.SetText("YOUR MOVE");
        _turnText.color = Color.white;

        for (int i = 0; i < 2; ++i)
        {
            if (playerOptions[i])
            {
                _valves[i].Rotate();
                ServiceLocator.Get<SoundManager>().PlaySound("PipeTurning");
            }
        }

        yield return new WaitForSeconds(2);

        for (int i = 0; i < 2; ++i)
        {
            if (prisonerOptions[i])
            {
                _valves[i].Rotate();
                ServiceLocator.Get<SoundManager>().PlaySound("PipeTurning");
            }
        }

        _turnText.gameObject.SetActive(true);
        _turnText.SetText("PRISONER MOVE");
        _turnText.color = Color.red;

        yield return new WaitForSeconds(2.5f);

        // Values are locked in
        valveValues[0] = Mathf.RoundToInt(_valves[0].transform.rotation.y) == 0;
        valveValues[1] = Mathf.RoundToInt(_valves[1].transform.rotation.y) == 0;

        _turnText.gameObject.SetActive(false);

        if (valveValues[0] == valveValues[1])
        {
            if (valveValues[0])
            {
                int health = --ServiceLocator.Get<Player>().Lives;
                var playerHealthsAngle = _playerPointer.transform.localRotation.eulerAngles;
                _playerPointer.transform.localRotation = Quaternion.Euler(playerHealthsAngle.x, playerHealthsAngle.y, playerHealthsAngle.z - 30f);
                StartCoroutine(ServiceLocator.Get<ParticleManager>().ActivateGasEffect(2f));
                ServiceLocator.Get<SoundManager>().PlaySound("PlayerLose");
                ServiceLocator.Get<SoundManager>().PlaySound("PipeSuccess");
                ServiceLocator.Get<VisualEffects>().SetBlur(health);

                if (health <= 0)
                {
                    FinishGame("Ai");
                }

                CheckMusic(health);
                _playerGotGassed = true;
            }
            else
            {
                --_AI_Health;
                var aiHealthAngle = _AIPointer.transform.localRotation.eulerAngles;
                _AIPointer.transform.localRotation = Quaternion.Euler(aiHealthAngle.x, aiHealthAngle.y, aiHealthAngle.z - 30f);
                ServiceLocator.Get<SoundManager>().PlaySound("PrisonerLose");
                ServiceLocator.Get<SoundManager>().PlaySound("PipeSuccess");

                if (_AI_Health <= 0)
                {
                    FinishGame("Player");
                }
            }
        }

        prisonerOptions = prisoner.GetNextMove(valveValues[0], valveValues[1]);
        _timer = _timePerRound;

        TellStory();
    }

    private void BlinkingTimer()
    {
        if (_blinkingArrowTimer <= 0.0f)
        {
            _blinkingArrowTimer = _blinkingTime;
            for (int i = 0; i < 2; ++i)
            {
                _valves[i].TimerForArowBlink(_shouldBlink);
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
        _valves[0].EnableValves();
        _valves[1].EnableValves();
        _gameOnGoing = true;
        ServiceLocator.Get<SoundManager>().PlaySound("RoundStart");
        ServiceLocator.Get<CursorClass>().SetPipesTurningToFalse();
        ServiceLocator.Get<CursorClass>().ReturnCursorToNormal();
    }

    private void TellStory()
    {
        if (_AI_Health  == 5)
        {
            if (_storiesMidHealth.Count > 0)
            {
                var story = _storiesMidHealth[Random.Range(0, _storiesMidHealth.Count)];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                return;
            }
        }
        else if(_AI_Health >= 3)
        {
            if (_playerGotGassed)
            {
                _playerGotGassed = false;
                if (_playerGassed[_AI_Health - 1])
                {
                    var story = _playerGassed[_AI_Health - 1];
                    ServiceLocator.Get<TextManager>().EnableStory(story);
                    return;
                }
            }

            int randomNum = Random.Range(0, 101);
            if (randomNum > 50 && _storiesMidHealth.Count > 0)
            {
                var story = _storiesMidHealth[Random.Range(0, _storiesMidHealth.Count)];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                return;
            }
            else if (_notTurningHint[_AI_Health - 2] && randomNum % 2 == 0)
            {
                var story = _notTurningHint[_AI_Health - 2];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                prisonerOptions[0] = false;
                prisonerOptions[1] = false;
                return;
            }
            else if (_turningHint[_AI_Health - 2])
            {
                var story = _turningHint[_AI_Health - 2];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                prisonerOptions[0] = true;
                prisonerOptions[1] = true;
                return;
            }
        }
        else if(_AI_Health >= 2)
        {
            if (_playerGotGassed)
            {
                _playerGotGassed = false;
                if (_playerGassed[_AI_Health - 1])
                {
                    var story = _playerGassed[_AI_Health - 1];
                    ServiceLocator.Get<TextManager>().EnableStory(story);
                    return;
                }
            }

            int randomNum = Random.Range(0, 101);
            if (randomNum > 50 && _storiesLowHealth.Count > 0)
            {
                var story = _storiesLowHealth[Random.Range(0, _storiesLowHealth.Count)];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                return;
            }
            else if (_notTurningHint[_AI_Health - 2] && randomNum % 2 == 0)
            {
                var story = _notTurningHint[_AI_Health - 2];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                prisonerOptions[0] = false;
                prisonerOptions[1] = false;
                return;
            }
            else if (_turningHint[_AI_Health - 2])
            {
                var story = _turningHint[_AI_Health - 2];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                prisonerOptions[0] = true;
                prisonerOptions[1] = true;
                return;
            }
        }
        else if(_AI_Health == 1)
        {
            if (_playerGotGassed)
            {
                _playerGotGassed = false;
                if (_playerGassed[_AI_Health - 1])
                {
                    var story = _playerGassed[_AI_Health - 1];
                    ServiceLocator.Get<TextManager>().EnableStory(story);
                    return;
                }
            }

            int randomNum = Random.Range(0, 101);
            if (randomNum > 50 && _storiesLowHealth.Count > 0)
            {
                var story = _storiesLowHealth[Random.Range(0, _storiesLowHealth.Count)];
                ServiceLocator.Get<TextManager>().EnableStory(story);
                return;
            }
        }

        ServiceLocator.Get<TextManager>().EnableStory(null);
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
            case 3:
                {
                    ServiceLocator.Get<SoundManager>().PlayMainSound("Ending");
                }
                return;
        }
    }

    private void FinishGame(string winner)
    {
        _gameOver = true;
        if (winner == "Player")
        {
            _escapeEnding.SetActive(true);
            ServiceLocator.Get<SoundManager>().PlayMainSound("Win");
        }
        else
        {
            _gameOverEnding.SetActive(true);
            ServiceLocator.Get<SoundManager>().PlayMainSound("Lose");
        }
    }
}
