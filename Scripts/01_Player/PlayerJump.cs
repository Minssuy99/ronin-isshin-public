using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15.0f;
    
    private bool _jumpEnabled = true;
    private bool _jumpRequested = false;
    
    private Rigidbody2D _rb;
    private PlayerGroundChecker _playerGroundChecker;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
    }
    
    private void Update()
    {
        HandleJump();
    }
    
    void OnJump()
    {
        if (_playerGroundChecker.isGrounded)
        {
            _jumpRequested = true;
        }
    }
    
    private void HandleJump()
    {
        if (_jumpRequested && _jumpEnabled && _playerGroundChecker.isGrounded)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
            _jumpRequested = false;
            Sound_Mng.Instance.PlaySFX("Jump"); 
        }
    }
    
    public void SetJumpEnabled(bool enable)
    {
        _jumpEnabled = enable;
    }
}
