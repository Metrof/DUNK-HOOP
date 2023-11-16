using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModel
{
    private UIView _view;

    private int _currentScore;
    private int _currentHealth;

    public UIModel(UIView view)
    {
        _view = view;
    }
    public void SetModelScore(int score)
    {
        _currentScore = score;
        _view.ChangeScoreView(_currentScore);
    }
    public void SetModelHealth(int health)
    {
        _currentHealth = health;
        _view.ChangeHealthView(_currentHealth);
    }
}
