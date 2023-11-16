using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BallStats", order = 1)]
public class BallTypeSettings : ScriptableObject
{
    [SerializeField] private Material _material;
    [SerializeField] private float _jumpDuration = 1.5f;
    [SerializeField] private float _jumpPower = 4;
    [SerializeField] private float _lifeTimeAfterAnim = 3;
    [SerializeField] private Ease _ballFlyEase = Ease.Linear;
    [SerializeField] private Ease _ballScaleEase = Ease.Linear;
    [SerializeField] private BallTypes _ballType = BallTypes.Normal;

    public Material Material { get { return _material; } }
    public float JumpDuration { get { return _jumpDuration;} }
    public float JumpPower { get { return _jumpPower;} }
    public float LifeTimeAfterAnim { get {  return _lifeTimeAfterAnim; } }
    public Ease BallFlyEase { get {  return _ballFlyEase; } }
    public Ease BallScaleEase { get { return  _ballScaleEase; } }
    public BallTypes BallType { get {  return _ballType; } }

}
public enum BallTypes
{
    Normal,
    Fire,
    InQuint
}
