using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObjManager _gameObjManager;
    [SerializeField] private GameLoop _gameLoopManager;
    [SerializeField] private Player _player;

    void Awake()
    {
        ServiceLocator.Register<GameObjManager>(_gameObjManager);
        ServiceLocator.Register<GameLoop>(_gameLoopManager);
        ServiceLocator.Register<Player>(_player);

        _gameLoopManager.Load();
    }
}
