using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface ICannonTerm 
{
    public int Prioritet { get; }
    public bool IsCorrectTerm(GameManager manager);
    public UniTask TermFire(Pool<Ball> balls, Dictionary<BallTypes, BallTypeSettings> settings, CancellationToken token);
}
public struct CannonFireParam
{
    public int ballCount;
    public BallTypes ballType;
    public CannonFireParam(int count,  BallTypes ballType)
    {
        this.ballCount = count;
        this.ballType = ballType;
    }
}
