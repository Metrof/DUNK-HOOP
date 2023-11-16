using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ring : MonoBehaviour
{
    [SerializeField] private float _moveDistanceX = 2;
    [SerializeField] private float _moveDistanceY = 1;
    [SerializeField] private float _speed = 1;
    [SerializeField] private RingTrigger _ringTrigger;
    [SerializeField] private Transform _textSpawnPoint;
    [SerializeField] private FlyingText _scoreFloatingTextPrefab;

    Controller _controller;
    Transform _transform;
    Cloth _cloth;
    FlyingText _flyingText;

    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;

    public float MoveDistanceX {  get { return _moveDistanceX; } }
    public RingTrigger TriggerAction { get { return _ringTrigger; } }

    private void Awake()
    {
        _transform = transform;
        _controller = new Controller();
        _cloth = GetComponentInChildren<Cloth>();
        _flyingText = Instantiate(_scoreFloatingTextPrefab);

        _minX = _transform.position.x - _moveDistanceX;
        _maxX = _transform.position.x + _moveDistanceX;
        _minY = _transform.position.y - _moveDistanceY;
        _maxY = _transform.position.y + _moveDistanceY;
    }

    private void OnEnable()
    {
        _controller.Enable();
        _controller.Player.MouseDelta.performed += Move;
    }
    private void OnDisable()
    {
        _controller.Player.MouseDelta.performed -= Move;
        _controller.Disable();
    }
    public void EnableMouse()
    {
        _controller.Enable();
    }
    public void DisableMouse()
    {
        _controller.Disable();
    }
    public void SetClothCollider(List<SphereCollider> colliders)
    {
        var pairs = new ClothSphereColliderPair[colliders.Count];
        for (int i = 0; i < pairs.Length; i++)
        {
            pairs[i].first = colliders[i];
            pairs[i].second = null;
        }
        _cloth.sphereColliders = pairs;
    }
    public void StartFloatText(int score)
    {
        _flyingText.transform.position = _textSpawnPoint.position;
        _flyingText.SrartFlying(score);
    }
    private void Move(InputAction.CallbackContext context)
    {
        Vector3 nextPos = _transform.position;
        nextPos.x += context.ReadValue<Vector2>().x * Time.deltaTime * _speed;
        nextPos.y += context.ReadValue<Vector2>().y * Time.deltaTime * _speed;
        nextPos.x = Mathf.Clamp(nextPos.x, _minX, _maxX);
        nextPos.y = Mathf.Clamp(nextPos.y, _minY, _maxY);
        _transform.position = nextPos;
    }
}
