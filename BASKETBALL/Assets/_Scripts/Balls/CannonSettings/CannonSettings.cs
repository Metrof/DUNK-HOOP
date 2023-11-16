using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CannonStats", order = 2)]
public class CannonSettings : ScriptableObject
{
    [SerializeField] private float _shotDellay = 1;
    [SerializeField] private int _ballPoolSize = 5;
    [SerializeField] private List<BallTypeSettings> _settings = new List<BallTypeSettings>();

    private List<ICannonTerm> _cannonTerms = new List<ICannonTerm>()
    {
        new InQuintTerm(),
        new FireBallTerm(),
        new TripleFireBallTerm(),
        new TripleNormalBallTerm(),
    };

    public int BallPoolSize { get { return _ballPoolSize; } }
    public float BaseShotDellay { get { return _shotDellay; } }
    public List<BallTypeSettings> BallTypeSettings { get { return _settings; } }
    public List<ICannonTerm> CannonTerms { get { return _cannonTerms; } }
}
