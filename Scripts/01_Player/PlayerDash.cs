using System;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.2f;

    public bool _isDashing { get; private set; }
    private bool _dashEnabled = true;
    private float _dashTimeLeft;
    private float _dashCooldownLeft;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
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
        if (!_isDashing && _dashEnabled && _dashCooldownLeft <= 0)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        _isDashing = true;
        _dashTimeLeft = dashDuration;
        _dashCooldownLeft = dashCooldown;
        _animator.SetTrigger("isDash");
        Sound_Mng.Instance.PlaySFX("Dash");
    }

    void EndDash()
    {
        _isDashing = false;
    }

    public void SetDashEnabled(bool enable)
    {
        _dashEnabled = enable;
    }
}
