using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Canvas _gameCanvas;
    [SerializeField] private GameSceneCanvas _gameSceneCanvasPrefab;
    public override void InstallBindings()
    {
        CanvasBinding();
    }
    private void CanvasBinding()
    {
        GameSceneCanvas gameSceneCanvas = Instantiate(_gameSceneCanvasPrefab, _gameCanvas.transform);
        Container
            .Bind<GameSceneCanvas>()
            .FromInstance(gameSceneCanvas)
            .AsSingle()
            .NonLazy();
    }
}