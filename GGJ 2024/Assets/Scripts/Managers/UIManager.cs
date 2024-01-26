using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    
    public void ButtonSceneLoader(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void ButtonPanelActive(GameObject _panel)
    {
        if (_panel.activeInHierarchy == true)
        {
            _panel.SetActive(false);
            _mainMenu.SetActive(true);
        }
        else
        {
            _panel.SetActive(true);
            _mainMenu.SetActive(false);
        }
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
}
