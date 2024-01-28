using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private string sceneToLoad = "";
    private Action actionForFade;

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
        sceneToLoad = _scene;
        Debug.Log(_animator);
        _animator.SetTrigger("FadeOut");
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
