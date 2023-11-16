using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TripleFireBallTerm : ICannonTerm
{
    private bool _isPrevios = false;
    public int Prioritet { get { return 2; } }

    public bool IsCorrectTerm(GameManager manager)
    {
        if (_isPrevios)
        {
            _isPrevios = false;
            return false;
        }
        if (manager.BallSeriesOfHit == 0) return false;
        return manager.IsSeriesSix;
    }
    public async UniTask TermFire(Pool<Ball> balls, Dictionary<BallTypes, BallTypeSettings> settings, CancellationToken token)
    {
        _isPrevios = true;
        for (int i = 0; i < 3; i++)
        {
            if (i != 0) await UniTask.Delay(TimeSpan.FromSeconds(2), cancellationToken: token);
            if (balls.TryInstantiate(out Ball ball, Vector3.zero, Quaternion.identity))
            {
                if (BallTypes.Fire != ball.GetBallType) { ball.SetSettings(settings[BallTypes.Fire]); }
                ball.Catching();
            }
        }
        await UniTask.Yield();
    }
}
