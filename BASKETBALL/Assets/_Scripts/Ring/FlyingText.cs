using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(TextMeshPro))]
public class FlyingText : MonoBehaviour
{
    [SerializeField] private float _flyingDuration = 1;
    [SerializeField] private float _flyingDistance = 1;
    [SerializeField] private Ease _flyingEase = Ease.Linear;
    [SerializeField] private Ease _fadeEase = Ease.Linear;

    private TextMeshPro _text;
    private Tween _flyingTween;
    private Tween _fadeTween;

    private Color _normalColor;

    private void Awake()
    {
        _text = GetComponent<TextMeshPro>();
        Color color = _text.color;
        _normalColor = color;
        color.a = 0;
        _text.color = color;
    }
    public void SrartFlying(int score)
    {
        _text.text = "+" + score;
        _text.color = _normalColor;
        _flyingTween.Kill();
        _fadeTween.Kill();
        _flyingTween = transform.DOMoveY(transform.position.y + _flyingDistance, _flyingDuration).SetEase(_flyingEase);
        _fadeTween = _text.DOFade(0, _flyingDuration).SetEase(_fadeEase);
    }
}
