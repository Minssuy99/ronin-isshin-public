using System;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.2f;

    public bool _isDashing { get; private set; }
    public float DashTimeLeft => _dashTimeLeft;
    private bool _dashEnabled = true;
    private float _dashTimeLeft;
    private float _dashCooldownLeft;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerStateManager _playerStateManager;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _playerStateManager = GetComponent<PlayerStateManager>();
    }

    private void FixedUpdate()
    {
        if (!_isDashing) return;
        
        float direction = transform.localScale.x;
        _rb.linearVelocity = new Vector2(direction * dashSpeed, 0);
    }

    private void Update()
    {
        if (_isDashing)
        {
            _dashTimeLeft -= Time.deltaTime;

            if (_dashTimeLeft <= 0)
            {
                EndDash();
            }
        }
        _dashCooldownLeft -=  Time.deltaTime;
    }

    void OnDash()
    {
        if (!_playerStateManager.CanDash()) return;

        if (!_isDashing && _playerGroundChecker.isGrounded && _dashEnabled && _dashCooldownLeft <= 0)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        _isDashing = true;
        _playerStateManager.IsDashing = true;
        _dashTimeLeft = dashDuration;
        _dashCooldownLeft = dashCooldown;
        _animator.SetTrigger("isDash");
        Sound_Mng.Instance.PlaySFX("Dash");
    }

    void EndDash()
    {
        _isDashing = false;
        _playerStateManager.IsDashing = false;
    }

    public void SetDashEnabled(bool enable)
    {
        _dashEnabled = enable;
    }
}