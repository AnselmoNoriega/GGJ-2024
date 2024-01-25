using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPauseMenu(panel);
            Debug.Log("Opening");
        }
    }
    public void SceneLoader(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void OpenPauseMenu(GameObject _panel)
    {
        if (panel.activeInHierarchy == true)
        {
            _panel.SetActive(false);
        }

        else
        {
            _panel.SetActive(true);
        }
    }
}
