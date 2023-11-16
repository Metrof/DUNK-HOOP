using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FireBallTerm : ICannonTerm
{
    private bool _isPrevios = false;
    public int Prioritet { get { return 1; } }
    public bool IsCorrectTerm(GameManager manager)
    {
        if (_isPrevios)
        {
            _isPrevios = false;
            return false;
        }
        if (manager.BallSeriesOfHit == 0) return false;
        return manager.IsSeriesTriple;
    }
    public async UniTask TermFire(Pool<Ball> balls, Dictionary<BallTypes, BallTypeSettings> settings, CancellationToken token)
    {
        _isPrevios = true;
        if (balls.TryInstantiate(out Ball ball, Vector3.zero, Quaternion.identity))
        {
            if (BallTypes.Fire != ball.GetBallType) { ball.SetSettings(settings[BallTypes.Fire]); }
            ball.Catching();
        }
        await UniTask.Yield();
    }
}
