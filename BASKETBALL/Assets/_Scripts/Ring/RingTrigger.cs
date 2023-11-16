using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RingTrigger : MonoBehaviour
{
    public Action OnCounterPluss;
    public Action OnDefaultBallHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.TryGetComponent(out Ball ball);
            if(ball.GetBallType == BallTypes.Normal || ball.GetBallType == BallTypes.InQuint) OnCounterPluss?.Invoke();
            ball.SetHit(true);
            OnDefaultBallHit?.Invoke();
        }
    }
}
