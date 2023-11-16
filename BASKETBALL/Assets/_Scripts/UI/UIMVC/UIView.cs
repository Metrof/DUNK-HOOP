using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIView 
{
    private GameSceneCanvas _gameSceneCanvas;

    [Inject]
    private void Construct(GameSceneCanvas gameSceneCanvas)
    {
        _gameSceneCanvas = gameSceneCanvas;
        _gameSceneCanvas.RestartButton.HideButton();
    }
    public void StartGameInit()
    {
        _gameSceneCanvas.StartButton.HideButton();
        _gameSceneCanvas.RestartButton.HideButton();
    }
    public void StopGameInit()
    {
        _gameSceneCanvas.RestartButton.ShowButton();
    }
    public void ChangeHealthView(int health)
    {
        _gameSceneCanvas.HealthText.text = health.ToString();
    }
    public void ChangeScoreView(int score)
    {
        _gameSceneCanvas.ScoreText.text = score.ToString();
    }
    public void ShowCrossView(Vector2 point)
    {
        _gameSceneCanvas.LoseCross.ShowLoseCross(point);
    }
}
