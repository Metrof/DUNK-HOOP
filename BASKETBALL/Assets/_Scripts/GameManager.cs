using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _playerMaxHealth = 3;

    [Header("Cannon")]
    [SerializeField] private CannonSettings _cannonSettings;
    [SerializeField] private int _scoreForBallAcceleration = 10;
    [SerializeField] private float _ballAcceleration = 0.01f;

    [Header("Prefabs")]
    [SerializeField] private Ring _ring;
    [SerializeField] private Ball _ballPrefab;
    [SerializeField] private Transform _ballAnchor;
    [SerializeField] private RectTransform _canvasRectTransform;
    [SerializeField] private Canvas _canvas;

    int _currentHealth;
    int _currentScore;
    int _ballSeriesOfHit;
    bool _isInputEnable;

    private BallCannon _ballCannon;
    private UIController _uiController;

    bool _isSeriesTriple;
    bool _isSeriesSix;
    bool _isSeriesTen;

    public bool IsSeriesTriple { get {  return _isSeriesTriple; } }
    public bool IsSeriesSix { get { return _isSeriesSix; } }
    public bool IsSeriesTen { get { return _isSeriesTen; } }

    public CannonSettings CannonSettings { get { return _cannonSettings; } }
    public Ball BallPrefab {  get { return _ballPrefab; } }
    public Ring Ring { get { return _ring; } }
    public Transform BallAnchor { get { return _ballAnchor; } }
    private int CurrentHealth 
    { 
        get { return _currentHealth; } 
        set 
        { 
            if (value >= 0)
            {
                _currentHealth = value;
                _uiController.SetHealth(_currentHealth);
            }
        } 
    }
    public int CurrentScore
    {
        get { return _currentScore; }
        private set
        {
            if (value > _currentScore)
            {
                _ring.StartFloatText(1);
            }
            _currentScore = value;
            if(_currentScore % _scoreForBallAcceleration == 0) _ballCannon.IncreaseShotDelay(_ballAcceleration);
            _uiController.SetScore(_currentScore);
        }
    }
    public int BallSeriesOfHit { get { return _ballSeriesOfHit;  } 
        private set 
        {
            _ballSeriesOfHit = value;
            if (_ballSeriesOfHit == 3) _isSeriesTriple = true;
            if (_ballSeriesOfHit == 6) _isSeriesSix = true;
            if (_ballSeriesOfHit == 10) _isSeriesTen = true;
        } 
    }

    private GameSceneCanvas _gameSceneCanvas;

    [Inject]
    private void Construct(DiContainer container, GameSceneCanvas gameSceneCanvas)
    {
        _gameSceneCanvas = gameSceneCanvas;
        _gameSceneCanvas.StartButton.OnClickButton += StartGame;
        _gameSceneCanvas.RestartButton.OnClickButton += StartGame;

        _ballCannon = new BallCannon(this);

        _uiController = container.Instantiate<UIController>();
    }
    private void OnDisable()
    {
        _gameSceneCanvas.StartButton.OnClickButton -= StartGame;
        _gameSceneCanvas.RestartButton.OnClickButton -= StartGame;
    }
    private void Start()
    {
        _ring.TriggerAction.OnCounterPluss += () => BallSeriesOfHit++;
        _ring.TriggerAction.OnDefaultBallHit += PlayerHit;
        _ring.SetClothCollider(_ballCannon.SphereColliders);
    }

    public void ResetToZeroBallSeries()
    {
        BallSeriesOfHit = 0;
    }
    public void ResetBools()
    {
        _isSeriesTriple = false;
        _isSeriesSix = false;
        _isSeriesTen = false;
    }


    private void StartGame()
    {
        _currentScore = 0;
        _currentHealth = _playerMaxHealth;
        _uiController.StartNewGame(_currentScore, _playerMaxHealth);

        _ring.EnableMouse();
        _ballCannon.StartCatchBall();
        _isInputEnable = true;
    }
    public void StopGame()
    {
        _uiController.StopGame();
    }
    private void DisableGame()
    {
        _ballCannon.StopCatchBall();
        _ring.DisableMouse();
        _isInputEnable = false;
    }
    public void CheckGame()
    {
        if (_currentHealth <= 0)
        {
            if (_isInputEnable) DisableGame();
            if (_ballCannon.CurentCountBallInAir <= 0) StopGame();
        }
    }
    public void PlayerMiss(Vector3 ballPos)
    {
        _ballSeriesOfHit = 0;
        CurrentHealth--;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, ballPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, screenPoint, _canvas.worldCamera, out Vector2 localPoint);
        _uiController.ShowCross(localPoint);
    }
    private void PlayerHit() => CurrentScore++;
}
