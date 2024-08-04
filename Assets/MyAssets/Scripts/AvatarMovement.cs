using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class AvatarMovement : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerActions _playerActions;
    private InputAction walk;
    private Vector2 _movement;
    [SerializeField] private float _speed = 10;
    private Rigidbody rb;

    private void Awake()
    {
        _playerActions = new PlayerActions();
    }

    private void Start()
    {
        rb = _player.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        walk = _playerActions.Movement.Walk;
        walk.Enable();
    }

    private void OnDestroy()
    {
        walk.Disable();
    }

    private void Update()
    {
        _movement = walk.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(_movement.x * _speed * Time.fixedDeltaTime, 0.0f,_movement.y *_speed* Time.fixedDeltaTime);
    }
}
