using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    
    private void FixedUpdate()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out LivingEntity livingEntity))
        {
            livingEntity.Damage(1);
            Destroy(gameObject );
        }
    }
}
