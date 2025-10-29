using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Attacking,
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 10.0f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isAttacking = false;

    private PlayerState currentState = PlayerState.Idle;

    /* Events */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        UpdateState();
        UpdateFlip();
        UpdateMovementAnimation();
        UpdateJumpAnimation();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!isAttacking && isGrounded)
            {
                animator.SetTrigger("isAttack");
                StartCoroutine(Attack());
            }
        }
    }
    void FixedUpdate()
    {
        if(!isAttacking)
           rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
    
    /* Collision */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    
    /* Function */
    void UpdateState()
    {
        if (isAttacking)
        {
            currentState = PlayerState.Attacking;
            return;
        }
        
        switch (currentState)
        {
            case PlayerState.Jumping:
                if (isGrounded)
                {
                    currentState = PlayerState.Idle;
                }
                break;
            case PlayerState.Idle:
                if (Mathf.Abs(moveInput.x) > 0.1f)
                {
                    currentState = PlayerState.Moving;
                }
                break;
            case PlayerState.Moving:
                if (Mathf.Abs(moveInput.x) <= 0.1f)
                {
                    currentState = PlayerState.Idle;
                }
                break;
            case PlayerState.Attacking:
                if (Mathf.Abs(moveInput.x) > 0.1f)
                {
                    currentState = PlayerState.Idle;
                }
                break;
        }
    }
    void UpdateFlip()
    {
        if (isAttacking) return;
        
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    void UpdateMovementAnimation()
    {
        if (isGrounded)
        {
            if(Mathf.Abs(moveInput.x) > 0.1f)
                animator.SetBool("isRun", true);
            else
                animator.SetBool("isRun", false);
        }
    }
    void UpdateJumpAnimation()
    {
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        
        animator.SetBool("isGrounded", isGrounded);
    }
    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump()
    {
        if (isAttacking) return;
        
        if (isGrounded)
        {
            if(currentState == PlayerState.Idle || currentState == PlayerState.Moving)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }
    IEnumerator Attack()
    {
        isAttacking = true;
        
        yield return new WaitForSeconds(0.3f);
        hitBox.enabled = false;
        
        yield return new WaitForSeconds(0.2f);
        
        isAttacking = false;
    }

    public void EnableHitbox()
    {
        hitBox.enabled = true;
    }
}