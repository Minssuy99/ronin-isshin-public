using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState currentState { get; private set; } = PlayerState.Idle;
    
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;

    
    private void Awake()
    {
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
    }
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        ControlMovement();
    }

    void UpdateState()
    {
        if (_playerAttack.isAttacking)
        {
            currentState = PlayerState.Attacking;
            return;
        }
        
        switch (currentState)
        {
            case PlayerState.Jumping:
                if (_playerGroundChecker.isGrounded)
                {
                    currentState = PlayerState.Idle;
                }
                break;
            case PlayerState.Idle:
                if (Mathf.Abs(_playerMovement.MoveInput.x) > 0.1f)
                {
                    currentState = PlayerState.Moving;
                }
                break;
            case PlayerState.Moving:
                if (Mathf.Abs(_playerMovement.MoveInput.x) <= 0.1f)
                {
                    currentState = PlayerState.Idle;
                }
                break;
            case PlayerState.Attacking:
                if (Mathf.Abs(_playerMovement.MoveInput.x) > 0.1f)
                {
                    currentState = PlayerState.Idle;
                }
                break;
        }
    }
    void ControlMovement()
    {
        if (_playerAttack.isAttacking)
        {
            _playerMovement.SetMovementEnabled(false);
            _playerMovement.SetFlipEnabled(false);
        }
        else
        {
            _playerMovement.SetMovementEnabled(true);
            _playerMovement.SetFlipEnabled(true);
        }
    }
}