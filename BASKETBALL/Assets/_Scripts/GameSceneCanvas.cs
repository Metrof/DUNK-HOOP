using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneCanvas : MonoBehaviour
{
    [SerializeField] private LoseCross _loseCross;
    [SerializeField] private StartButton _startButton;
    [SerializeField] private StartButton _restartButton;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _healthText;

    public LoseCross LoseCross { get {  return _loseCross; } }
    public StartButton StartButton {  get { return _startButton; } }
    public StartButton RestartButton { get { return _restartButton; } }
    public TextMeshProUGUI ScoreText { get {  return _scoreText; } }
    public TextMeshProUGUI HealthText { get { return _healthText; } }
}
