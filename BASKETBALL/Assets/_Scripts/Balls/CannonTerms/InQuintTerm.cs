using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InQuintTerm : ICannonTerm
{
    private int _minScore = 20;
    [Range(0, 100)] private int _chanse = 5;
    public int Prioritet { get { return 0; } }

    public bool IsCorrectTerm(GameManager manager)
    {
        return manager.CurrentScore == _minScore || UnityEngine.Random.Range(0, 100) <= _chanse;
    }
    public async UniTask TermFire(Pool<Ball> balls, Dictionary<BallTypes, BallTypeSettings> settings, CancellationToken token)
    {
        if (balls.TryInstantiate(out Ball ball, Vector3.zero, Quaternion.identity))
        {
            if (BallTypes.InQuint != ball.GetBallType) { ball.SetSettings(settings[BallTypes.InQuint]); }
            ball.Catching();
        }
        await UniTask.Yield();
    }
}
