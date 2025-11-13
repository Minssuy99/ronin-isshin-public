using System.Collections;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15.0f;

    private bool _jumpEnabled = true;
    private bool _jumpRequested = false;
    private Coroutine _jumpDelayCoroutine;

    private Rigidbody2D _rb;
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerStateManager _playerStateManager;
    private PlayerDash _playerDash;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _playerStateManager = GetComponent<PlayerStateManager>();
        _playerDash = GetComponent<PlayerDash>();
    }

    private void FixedUpdate()
    {
        HandleJump();
    }

    void OnJump()
    {
        if (!_playerStateManager.CanJump()) return;

        if (_playerGroundChecker.isGrounded)
        {
            if (_jumpDelayCoroutine != null)
            {
                StopCoroutine(_jumpDelayCoroutine);
            }

            _jumpDelayCoroutine = StartCoroutine(DelayedJumpRequest());
        }
    }

    IEnumerator DelayedJumpRequest()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        if (_playerStateManager.CanJump() && _playerGroundChecker.isGrounded)
        {
            _jumpRequested = true;
        }
    }

    private void HandleJump()
    {
        if (_playerStateManager.IsDashing && _playerDash.DashTimeLeft > 0.15f)
        {
            _jumpRequested = false;
            return;
        }

        if (_playerStateManager.IsAttacking)
        {
            _jumpRequested = false;
            return;
        }

        if (_jumpRequested && _jumpEnabled && _playerGroundChecker.isGrounded)
        {
            if (!_playerStateManager.CanJump())
            {
                _jumpRequested = false;
                return;
            }

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