using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerController _playerController;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        UpdateMovementAnimation();
        UpdateJumpAnimation();
    }

    void UpdateMovementAnimation()
    {
            if(_playerController.currentState == PlayerState.Moving)
                _animator.SetBool("isRun", true);
            else
                _animator.SetBool("isRun", false);
    }
    
    void UpdateJumpAnimation()
    {
        _animator.SetFloat("yVelocity", _rb.linearVelocity.y);
        
        _animator.SetBool("isGrounded", _playerGroundChecker.isGrounded);
    }
}