using UnityEngine;
using Zenject;

public class UIController 
{
    private UIModel _model;
    private UIView _view;

    private DiContainer _container;

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;

        _view = _container.Instantiate<UIView>();
        _model = new UIModel(_view);
    }
    public void StartNewGame(int score, int maxHealth)
    {
        _model.SetModelScore(score);
        _model.SetModelHealth(maxHealth);

        _view.StartGameInit();
    }
    public void StopGame()
    {
        _view.StopGameInit();
    }
    public void SetScore(int score)
    {
        _model.SetModelScore(score);
    }
    public void SetHealth(int changeValue)
    {
        _model.SetModelHealth(changeValue);
    }
    public void ShowCross(Vector2 crossPoint)
    {
        _view.ShowCrossView(crossPoint);
    }
}
