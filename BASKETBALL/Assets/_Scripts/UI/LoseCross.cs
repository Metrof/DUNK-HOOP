using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class LoseCross : MonoBehaviour
{
    [SerializeField] private float _scaleChangeDuration = 0.6f;
    [SerializeField] private float _shakeDuration = 0.4f;
    [SerializeField] private float _shakeForse = 0.5f;
    [SerializeField] private int _shakeVibrationCountInSec = 2;
    [SerializeField] private float _timeBetweenBornAndDie = 0.8f;

    [SerializeField] private float _crossHorPosBorderInProcent = 10;
    [SerializeField] private float _crossDownPosBorderInProcent = 10;

    private Image _image;
    private RectTransform _rectTransform;
    private float _rectWidth;
    private float _rectHeight;

    private float _canvasXSize;
    private float _canvasYSize;

    private Color _normalColor;
    private Color _fadeColor;

    private CancellationTokenSource _cancellationTokenSource;
    private Tween _scaleTween;
    private Sequence _seq;
    private void Awake()
    {
        _image = GetComponent<Image>();
        Color color = _image.color;
        _normalColor = color;
        color.a = 0;
        _fadeColor = color;
        _image.color = _fadeColor;

        var scaler = GetComponentInParent<CanvasScaler>();
        _canvasXSize = scaler.referenceResolution.x;
        _canvasYSize = scaler.referenceResolution.y;

        _rectTransform = GetComponent<RectTransform>();
        _rectWidth = _rectTransform.rect.width;
        _rectHeight = _rectTransform.rect.height;

    }

    public void ShowLoseCross(Vector2 localPoint)
    {
        _cancellationTokenSource?.Cancel();
        Vector2 startPoint = new Vector2(GetCorrectXPos(localPoint.x), GetCorrectYPos(localPoint.y));
        _rectTransform.localPosition = startPoint;
        _rectTransform.localScale = Vector2.zero;
        _image.color = _normalColor;

        _seq.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(_rectTransform.DOScale(1, _scaleChangeDuration).SetEase(Ease.Linear)); 
        _seq.Append(_rectTransform.DOShakeScale(_shakeDuration, new Vector3(_shakeForse, _shakeForse, 0), _shakeVibrationCountInSec, 0, false, ShakeRandomnessMode.Full).SetEase(Ease.Linear));
        _seq.AppendCallback(CrossLifeDelayAsync);
        _seq.SetEase(Ease.Linear);

    }
    private async void CrossLifeDelayAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            await ScaleAsync();
        }
        catch (OperationCanceledException)
        {
        }
    }
    private async UniTask ScaleAsync()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(_timeBetweenBornAndDie)).AttachExternalCancellation(_cancellationTokenSource.Token);
        _scaleTween.Kill();
        _scaleTween = _rectTransform.DOScale(0, _scaleChangeDuration).SetEase(Ease.Linear).OnComplete(PutAwayCross);
    }
    private void PutAwayCross()
    {
        _image.color = _fadeColor;
        _rectTransform.localScale = Vector3.one;
    }
    private float GetCorrectXPos(float currentX)
    {
        float procentFromXSize = (_canvasXSize / 100) * _crossHorPosBorderInProcent;
        float XMaxPos = (_canvasXSize / 2 - procentFromXSize) - _rectWidth;
        return Mathf.Clamp(currentX, -XMaxPos, XMaxPos);
    }
    private float GetCorrectYPos(float currentY)
    {
        float procentFromYSize = (_canvasYSize / 100) * _crossDownPosBorderInProcent;
        float YMaxPos = (_canvasYSize / 2 - procentFromYSize) - _rectHeight;
        return Mathf.Clamp(currentY, -YMaxPos, YMaxPos);
    }
}
