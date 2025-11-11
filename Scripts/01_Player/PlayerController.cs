using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerAnimState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
}

public class PlayerController : MonoBehaviour
{
    public PlayerAnimState currentState { get; private set; } = PlayerAnimState.Idle;
    
    private PlayerStateManager _playerStateManager;
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerMovement _playerMovement;
    private PlayerAttack _playerAttack;
    private PlayerJump _playerJump;
    private PlayerDash _playerDash;
    private PlayerThrow _playerThrow;

    
    private void Awake()
    {
        _playerStateManager = GetComponent<PlayerStateManager>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAttack = GetComponent<PlayerAttack>();
        _playerJump = GetComponent<PlayerJump>();
        _playerDash = GetComponent<PlayerDash>();
        _playerThrow = GetComponent<PlayerThrow>();
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
        if (_playerStateManager.IsAttacking)
        {
            currentState = PlayerAnimState.Attacking;
            return;
        }
        
        switch (currentState)
        {
            case PlayerAnimState.Jumping:
                if (_playerGroundChecker.isGrounded)
                {
                    currentState = PlayerAnimState.Idle;
                }
                break;
            case PlayerAnimState.Idle:
                if (Mathf.Abs(_playerMovement.MoveInput.x) > 0.1f)
                {
                    currentState = PlayerAnimState.Moving;
                }
                break;
            case PlayerAnimState.Moving:
                if (Mathf.Abs(_playerMovement.MoveInput.x) <= 0.1f)
                {
                    currentState = PlayerAnimState.Idle;
                }
                break;
            case PlayerAnimState.Attacking:
                if (Mathf.Abs(_playerMovement.MoveInput.x) > 0.1f)
                {
                    currentState = PlayerAnimState.Idle;
                }
                break;
        }
    }
    void ControlMovement()
    {
        if (_playerStateManager.IsAttacking ||
            _playerStateManager.IsDashing ||
            _playerStateManager.IsThrowing ||
            _playerStateManager.IsGuarding ||
            _playerStateManager.IsGuardHit ||
            _playerStateManager.IsHurt)
        {
            _playerMovement.SetMovementEnabled(false);
            _playerMovement.SetFlipEnabled(false);
            _playerJump.SetJumpEnabled(false);
            _playerDash.SetDashEnabled(false);
            _playerThrow.SetThrowEnabled(false);
        }
        else
        {
            _playerMovement.SetMovementEnabled(true);
            _playerMovement.SetFlipEnabled(true);
            _playerJump.SetJumpEnabled(true);
            _playerDash.SetDashEnabled(true);
            _playerThrow.SetThrowEnabled(true);
        }
    }
}