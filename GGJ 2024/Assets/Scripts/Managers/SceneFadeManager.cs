using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private string sceneToLoad = "";
    private Action actionForFade;
    [SerializeField] private bool fadeTransition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string _scene)
    {
        if (fadeTransition)
        {
            sceneToLoad = _scene;
            _animator.SetTrigger("FadeOut");
        }
        else
        {
            SceneManager.LoadScene(_scene);
        }
    }

    public void QuickFadeTransition(Action _action)
    {
        actionForFade = _action;
        _animator.SetTrigger("QuickFade");
    }

    private void OnFadeOut()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnQuickFade()
    {
        actionForFade?.Invoke();
    }
}
