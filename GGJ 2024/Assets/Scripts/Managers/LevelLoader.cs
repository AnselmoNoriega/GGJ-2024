using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObjManager _gameObjManager;

    void Awake()
    {
        ServiceLocator.Register<GameObjManager>(_gameObjManager);
    }
}
