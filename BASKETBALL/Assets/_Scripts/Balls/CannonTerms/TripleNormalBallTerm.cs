using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class TripleNormalBallTerm : ICannonTerm
{
    private bool _isPrevios = false;
    public int Prioritet { get { return 3; } }

    public bool IsCorrectTerm(GameManager manager)
    {
        if (_isPrevios)
        {
            _isPrevios = false;
            return false;
        }
        if (manager.BallSeriesOfHit == 0) return false;
        if (manager.IsSeriesTen)
        {
            manager.ResetToZeroBallSeries();
            return true;
        }
        return false;
    }
    public async UniTask TermFire(Pool<Ball> balls, Dictionary<BallTypes, BallTypeSettings> settings, CancellationToken token)
    {
        _isPrevios = true;
        for (int i = 0; i < 3; i++)
        {
            if (balls.TryInstantiate(out Ball ball, Vector3.zero, Quaternion.identity))
            {
                if (BallTypes.Normal != ball.GetBallType) { ball.SetSettings(settings[BallTypes.Normal]); }
                ball.Catching();
            }
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: token);
        }
        await UniTask.Yield();
    }
}
