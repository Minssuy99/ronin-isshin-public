using System;
using System.Collections;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwCooldown = 0.5f;
    
    private bool _throwEnabled = true;
    private float _throwCooldownLeft;
    private Animator _animator;
    private PlayerGroundChecker _playerGroundChecker;
    
    public bool isThrowing { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
    }
    
    private void Update()
    {
        _throwCooldownLeft -= Time.deltaTime;
    }

    void OnThrow()
    {
        if (_throwEnabled && _throwCooldownLeft <= 0)
        {
            if (_playerGroundChecker.isGrounded && !isThrowing)
            {
                StartCoroutine(ThrowCoroutine());
            }
        }
    }

    IEnumerator ThrowCoroutine()
    {
        isThrowing = true;
        
        _animator.SetTrigger("isThrow");
        Sound_Mng.Instance.PlaySFX("Throw");
        _throwCooldownLeft = throwCooldown;
        
        yield return new WaitForSeconds(0.4f);
        isThrowing = false;
    }

    public void ThrowProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        float direction = transform.localScale.x;
        projectile.GetComponent<Projectile>().Initialize(direction);
    }
    
    public void SetThrowEnabled(bool enable)
    {
        _throwEnabled = enable;
    }
}
