using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObjManager _gameObjManager;
    [SerializeField] private GameLoop _gameLoopManager;
    [SerializeField] private Player _player;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private PlayerInputs _playerInput;
    [SerializeField] private TextManager _textManager;
    [SerializeField] private ParticleManager _particleManager;
    [SerializeField] private UIManager _uIManager;
    [SerializeField] private VisualEffects _visualEffects;

    void Awake()
    {
        ServiceLocator.Register<GameObjManager>(_gameObjManager);
        ServiceLocator.Register<GameLoop>(_gameLoopManager);
        ServiceLocator.Register<Player>(_player);
        ServiceLocator.Register<SoundManager>(_soundManager);
        ServiceLocator.Register<PlayerInputs>(_playerInput);
        ServiceLocator.Register<TextManager>(_textManager);
        ServiceLocator.Register<ParticleManager>(_particleManager);
        ServiceLocator.Register<UIManager>(_uIManager);
        ServiceLocator.Register<VisualEffects>(_visualEffects);

        _gameLoopManager.Load();
        _playerInput.Load();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameObjManager>();
        ServiceLocator.Unregister<GameLoop>();
        ServiceLocator.Unregister<Player>();
        ServiceLocator.Unregister<SoundManager>();
        ServiceLocator.Unregister<ParticleManager>();
        ServiceLocator.Unregister<UIManager>();
        ServiceLocator.Unregister<VisualEffects>();
    }
}
