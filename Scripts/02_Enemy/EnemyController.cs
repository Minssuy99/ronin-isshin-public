using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private LayerMask detectionLayer;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stopDistance = 2f;

    private Rigidbody2D _rb;
    private Transform _playerTransform;
    private Animator _animator;
    private bool _isChasing;

    [SerializeField] private float awarenessIncreaseSpeed = 50f;
    [SerializeField] private float awarenessDecreaseSpeed = 20f;
    [SerializeField] private float awarenessThreshold = 100f;
    private float _awarenessLevel;

    [SerializeField] private Image awarenessGaugeImage;
    private bool _hasReachedMax = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (awarenessGaugeImage)
        {
            awarenessGaugeImage.fillAmount = _awarenessLevel / awarenessThreshold;
            
            if (awarenessGaugeImage.fillAmount >= 1f)
            {
                _hasReachedMax = true;
            }
            if (_awarenessLevel <= 0f)
            {
                _hasReachedMax = false;
            }
            if (_hasReachedMax)
            {
                awarenessGaugeImage.color = Color.red;
            }
            else
            {
                awarenessGaugeImage.color = Color.yellow;
            }
        }
        
        Vector2 frontDirection = transform.localScale.x < 0 ? Vector2.left : Vector2.right;
        Vector2 backDirection = transform.localScale.x < 0 ? Vector2.right : Vector2.left;
        
        RaycastHit2D frontHit = Physics2D.Raycast(transform.position, frontDirection, detectionRange, detectionLayer);
        Debug.DrawRay(transform.position, frontDirection * detectionRange, Color.magenta);
        
        RaycastHit2D backHit = Physics2D.Raycast(transform.position, backDirection, detectionRange,  detectionLayer);
        Debug.DrawRay(transform.position, backDirection * detectionRange, Color.yellow);

        if (frontHit.collider && frontHit.collider.CompareTag("Player"))
        {
            _awarenessLevel = awarenessThreshold;
            _isChasing = true;
        }
        else if (backHit.collider && backHit.collider.CompareTag("Player"))
        {
            _awarenessLevel +=  awarenessIncreaseSpeed * Time.deltaTime;
            _awarenessLevel = Mathf.Clamp(_awarenessLevel, 0, awarenessThreshold);
            Debug.DrawRay(transform.position, backDirection * detectionRange, Color.blue);
        }
        else
        {
            _awarenessLevel -=  awarenessDecreaseSpeed * Time.deltaTime;
            _awarenessLevel = Mathf.Clamp(_awarenessLevel, 0, awarenessThreshold);
            Debug.DrawRay(transform.position, backDirection * detectionRange, Color.red);
        }

        Debug.Log("is Chasing is " + _isChasing);
        Debug.Log("인식 게이지 : " + _awarenessLevel);

        // 게이지가 임계값 이상이면 추격 시작
        if (_awarenessLevel >= awarenessThreshold)
        {
            _isChasing = true;
        }

        // 게이지가 거의 0이면 추격 중단
        if (_awarenessLevel <= 0.5f)
        {
            _awarenessLevel = 0f;
            _isChasing = false;
        }
    }

    private void FixedUpdate()
    {
        if (_isChasing && _playerTransform)
        {
            // 플레이어와의 거리 체크
            float distance = Vector2.Distance(transform.position, _playerTransform.position);

            // stopDistance보다 멀면 이동
            if (distance > stopDistance)
            {
                _animator.SetBool("isRun", true);
                float dir = _playerTransform.position.x - transform.position.x;
                float direction = dir > 0 ? 1 : -1;
                _rb.linearVelocity = new Vector2(direction * moveSpeed, _rb.linearVelocity.y);

                transform.localScale = new Vector2(direction, 1);
            }
            else
            {
                // stopDistance 안에 들어오면 멈춤
                _animator.SetBool("isRun", false);
                _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
            }
        }
        else
        {
            _animator.SetBool("isRun", false);
            _rb.linearVelocity = new Vector2(0, _rb.linearVelocity.y);
        }
    }
}