using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using TMPro;

public class TextManager : MonoBehaviour
{
    [Header("Story Info")]
    private Story _story;

    [Space, Header("Word Info")]
    [SerializeField] private TextMeshProUGUI _storyText;
    [SerializeField] private float _wordSpeed = 0.05f;
    private float _timer = 0.0f;
    private bool _loadingText = false;
    private int _currentWord = 0;
    private string _currentStoryLine = "";

    [Space, Header("Sounds")]
    [Range(0, 5), SerializeField] private int _frequencyLevel;
    [Range(-3, 3), SerializeField] private float _minPitch;
    [Range(-3, 3), SerializeField] private float _maxPitch;

    public void EnableStory(TextAsset textStory)
    {
        if (textStory == null)
        {
            ServiceLocator.Get<GameLoop>().ContinueGame();
            ServiceLocator.Get<UIManager>().ButtonSetActive(true);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        _story = new Story(textStory.text);
        _story.BindExternalFunction("playSound", (string soundName) =>
        {
            ServiceLocator.Get<SoundManager>().PlaySound(soundName);
        });

        LoadTextAnim();
    }

    private void Update()
    {
        if (_loadingText)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0.0f && _currentWord < _story.currentText.Length)
            {
                PlaySound(_currentWord);
                _storyText.text += _currentStoryLine[_currentWord++];
                _timer = _wordSpeed;
            }
            else if (_currentWord >= _currentStoryLine.Length)
            {
                _loadingText = false;
            }
        }
    }

    public void OnClick(InputAction.CallbackContext input)
    {
        LoadTextAnim();
    }

    public void LoadTextAnim()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        if (!_loadingText && _story.canContinue)
        {
            _storyText.text = "";
            _loadingText = true;
            _currentWord = 0;
            _timer = _wordSpeed;
            _currentStoryLine = _story.Continue();
        }
        else if (!_loadingText && !_story.canContinue)
        {
            ServiceLocator.Get<UIManager>().ButtonSetActive(true);
            ServiceLocator.Get<GameLoop>().ContinueGame();
            gameObject.SetActive(false);
        }
        else
        {
            _loadingText = false;
            _storyText.text = _currentStoryLine;
        }
    }

    private void PlaySound(int wordCount)
    {
        if(wordCount % _frequencyLevel == 0)
        {
            ServiceLocator.Get<SoundManager>().StopSound("Talking");
            ServiceLocator.Get<SoundManager>().ChangePitch("Talking", Random.Range(_minPitch, _maxPitch));
            ServiceLocator.Get<SoundManager>().PlaySound("Talking");
        }
    }

    private void OnDisable()
    {
        _loadingText = false;
    }
}
