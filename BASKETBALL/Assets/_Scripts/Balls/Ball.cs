using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Ball : PooledItem
{
    public Action OnReturnToPool;
    public Action<Vector3> OnMiss;

    private BallTypeSettings _settings;

    Tween _jumpTween;
    Tween _scaleTween;
    Vector3 _defoltTargetPos;
    Vector3 _maxYPos;
    float _offsetX;
    float _offsetZ;
    float _distanceBetweenStartsAndEnd;
    bool _isHit;

    private CancellationTokenSource _cancellationTokenSource;

    Renderer _renderer;
    Rigidbody _rigidbody;
    SphereCollider _collider;

    public SphereCollider SphereCollider { get { return _collider; } }
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    public void Init(Vector3 target, float offset, float ringRadius)
    {
        _defoltTargetPos = target;
        _distanceBetweenStartsAndEnd = Vector3.Distance(transform.position, _defoltTargetPos);
        _offsetX = offset - ringRadius;
        _offsetZ = ringRadius;
    }
    public BallTypes GetBallType { get { return _settings != null ? _settings.BallType : BallTypes.Normal; } }
    public void SetSettings(BallTypeSettings settings)
    {
        _settings = settings;
        _renderer.material = _settings.Material;
    }
    public void Catching()
    {
        _jumpTween.Kill();
        _scaleTween.Kill();
        _isHit = false;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
        _rigidbody.position = transform.position;
        Vector3 endValue = _defoltTargetPos;
        endValue.x += UnityEngine.Random.Range(-_offsetX, _offsetX);

        _scaleTween = transform.DOScale(Vector3.one, _settings.JumpDuration).SetEase(_settings.BallScaleEase);
        _jumpTween = _rigidbody.DOJump(endValue, _settings.JumpPower, 1, _settings.JumpDuration)
            .SetEase(_settings.BallFlyEase)
            .OnComplete(StartAsyncBallLifeTime);
        _ = GetMaxYPointAsync();
    }
    public void SetHit(bool hit)
    {
        _isHit = hit;
    }
    private async UniTaskVoid GetMaxYPointAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_settings.JumpDuration / 2));
        _maxYPos = transform.position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        _jumpTween.Kill();
        StartAsyncBallLifeTime();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ring"))
        {
            _cancellationTokenSource?.Cancel();
            Return();
        }
    }
    private void Return()
    {
        if (!_isHit) OnMiss?.Invoke(transform.position);
        OnReturnToPool?.Invoke();
        _rigidbody.velocity = Vector3.zero;
        ReturnToPool();
    }
    private void StartAsyncBallLifeTime()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _ = BallLifeAsync(_cancellationTokenSource.Token);
    }
    private async UniTaskVoid BallLifeAsync(CancellationToken token)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_settings.LifeTimeAfterAnim), cancellationToken: token);
        if (_cancellationTokenSource.IsCancellationRequested) return;
        Return();
    }
}
