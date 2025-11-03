using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BoxCollider2D[] hitBoxes =  new BoxCollider2D[3];
    [SerializeField] private float comboWindowDuration = 1.5f;
    [SerializeField] private float attackAnimDuration = 0.27f;

    private PlayerGroundChecker _playerGroundChecker;
    private Animator _animator;

    private int currentCombo = 0;
    private Coroutine comboResetCoroutine;
    private const int MAX_COMBO = 3;
    
    public bool isAttacking { get; private set; }

    private void Awake()
    {
        _playerGroundChecker = GetComponent<PlayerGroundChecker>();
        _animator = GetComponent<Animator>();
    }

    void OnAttack()
    {
        if (!isAttacking && _playerGroundChecker.isGrounded)
        {
            if (currentCombo == 0)
            {
                StartCombo();
            }
            else if (currentCombo > 0 && currentCombo < MAX_COMBO)
            {
                NextCombo();
            }
        }
    }

    void StartCombo()
    {
        currentCombo = 1;
        _animator.SetInteger("comboCount", currentCombo);
        StartCoroutine(AttackCoroutine());
    }

    void NextCombo()
    {
        if (comboResetCoroutine != null)
        {
            StopCoroutine(comboResetCoroutine);
            comboResetCoroutine = null;
        }

        currentCombo++;
        _animator.SetInteger("comboCount", currentCombo);
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        switch (currentCombo)
        {
            case 1:
                Sound_Mng.Instance.PlaySFX("Slash_1");
                break;
            case 2:
                Sound_Mng.Instance.PlaySFX("Slash_2");
                break;
            case 3:
                Sound_Mng.Instance.PlaySFX("Slash_3");
                break;
        }

        yield return new WaitForSeconds(attackAnimDuration);
        
        _animator.SetInteger("comboCount", 0);
        
        isAttacking = false;

        if (currentCombo < MAX_COMBO)
        {
            comboResetCoroutine = StartCoroutine(ComboResetTimer());
        }
        else
        {
            ResetCombo();
        }
    }

    IEnumerator ComboResetTimer()
    {
        yield return new WaitForSeconds(comboWindowDuration);
        ResetCombo();
    }
    
    public void EnableHitbox()
    {
        if (currentCombo > 0 && currentCombo <= hitBoxes.Length)
        {
            hitBoxes[currentCombo - 1].enabled = true;
        }
    }

    public void DisableHitbox()
    {
        if (currentCombo > 0 && currentCombo <= hitBoxes.Length)
        {
            hitBoxes[currentCombo - 1].enabled = false;
        }
    }

    void ResetCombo()
    {
        if (comboResetCoroutine != null)
        {
            StopCoroutine(comboResetCoroutine);
            comboResetCoroutine = null;
        }

        currentCombo = 0;
        _animator.SetInteger("comboCount", 0);
    }
}
