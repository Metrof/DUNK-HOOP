using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BallCannon
{
    private CannonSettings _cannonSettings;
    private GameManager _gameManager;
    private float _shotDellay = 1;
    private Transform _ballAnchor;
    private Pool<Ball> _ballPool;
    private CancellationTokenSource _cancellationTokenSource;
    private List<SphereCollider> _ballColliders = new();
    private Dictionary<BallTypes, BallTypeSettings> _settings = new();

    private int _curentCountBallInAir;
    public int CurentCountBallInAir { get { return _curentCountBallInAir; } }
    public List<SphereCollider> SphereColliders {  get { return _ballColliders; } }
    public BallCannon(GameManager gameManager)
    {
        _gameManager = gameManager;
        _cannonSettings = _gameManager.CannonSettings;
        _shotDellay = _cannonSettings.BaseShotDellay;
        foreach (var item in _cannonSettings.BallTypeSettings)
        {
            _settings.Add(item.BallType, item);
        }
        _ballAnchor = _gameManager.BallAnchor;
        CreateBallPool(_gameManager);
    }
    private void CreateBallPool(GameManager gameManager)
    {
        List<Ball> list = new List<Ball>();
        for (int i = 0; i < _cannonSettings.BallPoolSize; i++)
        {
            Ball ball = GameObject.Instantiate(gameManager.BallPrefab, _ballAnchor);
            ball.Init(gameManager.Ring.transform.position, gameManager.Ring.MoveDistanceX, gameManager.Ring.transform.localScale.x / 2);
            ball.SetSettings(_settings[BallTypes.Normal]);
            ball.OnMiss += gameManager.PlayerMiss;
            ball.OnReturnToPool += () => _curentCountBallInAir--;
            ball.OnReturnToPool += gameManager.CheckGame;
            _ballColliders.Add(ball.SphereCollider);
            list.Add(ball);
        }
        _ballPool = new Pool<Ball>(list);
    }
    public void IncreaseShotDelay(float offset)
    {
        _shotDellay -= offset;
    }
    public void StartCatchBall()
    {
        _curentCountBallInAir = 0;
        _cancellationTokenSource = new CancellationTokenSource();
        _ = BallCatchAsync(_cancellationTokenSource.Token);
    }
    public void StopCatchBall()
    {
        _cancellationTokenSource?.Cancel();
    }
    private async UniTaskVoid BallCatchAsync(CancellationToken token)
    {
        do
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_shotDellay), cancellationToken: token);
            ICannonTerm activeTerm = null;
            foreach (var term in _cannonSettings.CannonTerms)
            {
                if (term.IsCorrectTerm(_gameManager))
                {
                    if (activeTerm != null)
                    {
                        if (activeTerm.Prioritet < term.Prioritet)
                        {
                            activeTerm = term;
                        }
                        continue;
                    }
                    activeTerm = term;
                }
            }
            if(activeTerm != null)
            {
                _gameManager.ResetBools();
                await activeTerm.TermFire(_ballPool, _settings, token);
            }
            else Fire();
        } while (!token.IsCancellationRequested);
    }
    private void Fire()
    {
        if (_ballPool.TryInstantiate(out Ball ball, Vector3.zero, Quaternion.identity))
        {
            if (BallTypes.Normal != ball.GetBallType) { ball.SetSettings(_settings[BallTypes.Normal]); }
            ball.Catching();
        }
    }
}
