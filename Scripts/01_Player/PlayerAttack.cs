using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider2D hitBox;
    private PlayerGroundChecker _playerGroundChecker;
    private Animator _animator;
    
    public bool isAttacking { get; private set; } = false;

    private void Awake()
    {
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _animator = GetComponent<Animator>();
    }

    void OnAttack()
    {
        if (!isAttacking && _playerGroundChecker.isGrounded)
        {
            _animator.SetTrigger("isAttack");
            StartCoroutine(AttackCoroutine());
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        
        yield return new WaitForSeconds(0.2f);
        
        isAttacking = false;
    }
    
    public void EnableHitbox()
    {
        hitBox.enabled = true;
    }

    public void DisableHitbox()
    {
        hitBox.enabled = false;
    }
}