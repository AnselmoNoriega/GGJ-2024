using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObjManager _gameObjManager;
    [SerializeField] private GameLoop _gameLoopManager;
    [SerializeField] private Player _player;
    [SerializeField] private SoundManager _soundManager;

    void Awake()
    {
        ServiceLocator.Register<GameObjManager>(_gameObjManager);
        ServiceLocator.Register<GameLoop>(_gameLoopManager);
        ServiceLocator.Register<Player>(_player);
        ServiceLocator.Register<SoundManager>(_soundManager);

        _gameLoopManager.Load();
    }
}
