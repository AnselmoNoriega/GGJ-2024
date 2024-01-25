using UnityEngine;
using Ink.Runtime;
using UnityEngine.InputSystem;
using TMPro;

public class TextManager : MonoBehaviour
{
    [Header("Story Info")]
    public TextAsset TextAsset;
    private Story _story;

    [Space, Header("Word Info")]
    [SerializeField] private TextMeshProUGUI _storyText;
    [SerializeField] private float _wordSpeed = 0.05f;
    private float _timer = 0.0f;
    private bool _loadingText = false;
    private int _currentWord = 0;
    private string _currentStoryLine = "";

    private void OnEnable()
    {
        _story = new Story(TextAsset.text);

        LoadTextAnim();
    }

    private void Update()
    {
        if (_loadingText)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0.0f && _currentWord < _story.currentText.Length)
            {
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
        if (!_loadingText && _story.canContinue)
        {
            _storyText.text = "";
            _loadingText = true;
            _currentWord = 0;
            _timer = _wordSpeed;
            _currentStoryLine = _story.Continue();
        }
        else if(!_loadingText && !_story.canContinue)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _loadingText = false;
            _storyText.text = _currentStoryLine;
        }
    }

    private void OnDisable()
    {
        _loadingText = false;
    }
}
