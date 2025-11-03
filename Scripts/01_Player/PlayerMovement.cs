using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float jumpForce = 15.0f;

    public Vector2 MoveInput { get; private set; }
    
    private Rigidbody2D _rb;
    private PlayerGroundChecker _playerGroundChecker;
    private bool _movementEnabled = true;
    private bool _flipEnabled = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();

    }
    private void Update()
    {
        UpdateFlip();
    }
    private void FixedUpdate()
    {
        if (_movementEnabled)
        {
            _rb.linearVelocity = new Vector2(MoveInput.x * moveSpeed, _rb.linearVelocity.y);
        }
        else
        {
            _rb.linearVelocity = new Vector2(0f, _rb.linearVelocity.y);
        }
    }
    void UpdateFlip()
    {
        if (!_flipEnabled) return;

        if (MoveInput.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (MoveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    void OnMove(InputValue value)
    {
        MoveInput = value.Get<Vector2>();
    }
    void OnJump()
    {
        if (_playerGroundChecker.isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        }
    }
    public void SetMovementEnabled(bool enabled)
    {
        _movementEnabled = enabled;
    }

    public void SetFlipEnabled(bool enabled)
    {
        _flipEnabled = enabled;
    }
}