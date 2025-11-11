using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGuard : MonoBehaviour
{
    [SerializeField] private BoxCollider2D hurtBoxCollider;
    [SerializeField] private float guardHitDuration = 0.3f;
    [SerializeField] private float hurtDuration = 0.4f;

    private Animator _animator;
    private PlayerGroundChecker _playerGroundChecker;
    private PlayerStateManager _playerStateManager;
    private Rigidbody2D _rb;
    private PlayerAttack _playerAttack;

    private float _currentGuardInput = 0f;
    private Coroutine _hurtCoroutine;
    private int _lastHurtFrame = -1;

      private void Awake()
      {
          _animator = GetComponent<Animator>();
          _playerGroundChecker = GetComponent<PlayerGroundChecker>();
          _playerStateManager = GetComponent<PlayerStateManager>();
          _rb = GetComponent<Rigidbody2D>();
          _playerAttack = GetComponent<PlayerAttack>();
      }

      void OnGuard(InputValue value)
      {
          _currentGuardInput = value.Get<float>();
      }

      void Update()
      {
          if (_playerStateManager.IsGuardHit || _playerStateManager.IsHurt) return;

          if (!_playerGroundChecker.isGrounded)
          {
              if (_playerStateManager.IsGuarding)
                  EndGuard();
              return;
          }

          if (_currentGuardInput > 0.5f && _playerStateManager.CanGuard() && !_playerStateManager.IsGuarding)
          {
              StartGuard();
          }
          else if (_currentGuardInput <= 0.5f && _playerStateManager.IsGuarding)
          {
              EndGuard();
          }
      }

      void StartGuard()
      {
          _playerStateManager.IsGuarding = true;
          _animator.SetBool("isGuard", true);
          _animator.SetTrigger("guardEnter");
      }

      void EndGuard()
      {
          _playerStateManager.IsGuarding = false;
          _animator.SetBool("isGuard", false);
      }

      void OnTriggerEnter2D(Collider2D collision)
      {
          // Debug.Log($" 충돌 오브젝트: {collision.gameObject.name}, Tag: {collision.tag}");
          CheckGuardCollision(collision);
      }

      void CheckGuardCollision(Collider2D collision)
      {
          if (collision.CompareTag("EnemyAttack"))
          {
              Transform enemyRoot = collision.transform.root;

              if (_playerStateManager.IsGuarding && IsAttackFromFront(enemyRoot.position))
              {
                  OnGuardHit();
              }
              else
              {
                  OnPlayerHurt();
              }
          }
      }

      void OnPlayerHurt()
      {
          if (_lastHurtFrame == Time.frameCount)
          {
              return;
          }
          _lastHurtFrame = Time.frameCount;

          if (_playerStateManager.IsGuarding)
          {
              EndGuard();
          }

          if (_playerStateManager.IsAttacking)
          {
              _playerAttack.CancelAttack();
          }

          if (_hurtCoroutine != null)
          {
              StopCoroutine(_hurtCoroutine);
          }
          _hurtCoroutine = StartCoroutine(HurtCoroutine());
      }

      IEnumerator HurtCoroutine()
      {
          _playerStateManager.IsHurt = true;
          _rb.linearVelocity = Vector2.zero;
          _animator.SetTrigger("isHurt");
          Sound_Mng.Instance.PlaySFX("Hit");

          yield return new WaitForSeconds(hurtDuration);

          _rb.linearVelocity = Vector2.zero;
          _playerStateManager.IsHurt = false;
          _hurtCoroutine = null;
      }

      bool IsAttackFromFront(Vector3 attackPosition)
      {
          float playerDirection = transform.localScale.x;

          float attackDirection = attackPosition.x - transform.position.x;

          // Debug.Log($"플레이어 방향: {playerDirection}, 공격 방향: {attackDirection}, 공격 위치: {attackPosition.x}, 플레이어 위치: {transform.position.x}");

          bool isFront = (playerDirection > 0 && attackDirection > 0) ||
                         (playerDirection < 0 && attackDirection < 0);

          // Debug.Log($"앞에서 온 공격 : {isFront}");

          return isFront;
      }

      void OnGuardHit()
      {
          StartCoroutine(GuardHitCoroutine());
      }

      IEnumerator GuardHitCoroutine()
      {
          _playerStateManager.IsGuardHit = true;
          _animator.SetTrigger("guardHit");
          Sound_Mng.Instance.PlaySFX("Parrying_1");
          yield return new WaitForSeconds(guardHitDuration);
          _playerStateManager.IsGuardHit = false;
      }
  }