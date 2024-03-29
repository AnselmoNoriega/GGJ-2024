using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private Button _readyButton;
    [SerializeField] private SceneFadeManager _sceneFadeManager;
    [SerializeField] private bool _shouldFadeButtons = false;
    
    public void ButtonSceneLoader(string _scene)
    {
        if (_shouldFadeButtons)
        {
            _sceneFadeManager.LoadScene(_scene);
        }
        else
        {
            SceneManager.LoadScene(_scene);
        }
        Time.timeScale = 1;
    }

    public void ButtonPanelActive(GameObject _panel)
    {
        if (_panel.activeInHierarchy == true)
        {
            if (!_shouldFadeButtons)
            {
                SetPanelActive(_panel, false);
            } else
            {
                _sceneFadeManager.QuickFadeTransition(() => SetPanelActive(_panel, false));
            }
        }
        else
        {
            if (!_shouldFadeButtons)
            {
                SetPanelActive(_panel, true);
            } else
            {
                _sceneFadeManager.QuickFadeTransition(() => SetPanelActive(_panel, true));
            }
        }
    }

    private void SetPanelActive(GameObject _panel, bool active)
    {
        _panel.SetActive(active);
        _mainMenu.SetActive(!active);
    }

    public void ButtonExitGame()
    {
        Application.Quit();
    }

    public void ButtonResumeGame(GameObject _pauseMenu)
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ButtonSetActive(bool active)
    {
        _readyButton.interactable = active;
    }
}
