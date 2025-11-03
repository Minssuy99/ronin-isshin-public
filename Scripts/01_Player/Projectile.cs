using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float rotationSpeed = 360f;
    
    private Rigidbody2D _rb;
    private float _direction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(_direction * speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(float direction)
    {
        _direction = direction;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
        _rb.angularVelocity = -rotationSpeed * direction;
    }
}
