using System;
using MelenitasDev.SoundsGood;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;

    private Sound bulletSound;
    public string owner;

    private void Awake()
    {
        bulletSound = new Sound(SFX.Shoot).SetSpatialSound(false).SetOutput(Output.SFX);
    }

    private void Start()
    {
        Destroy(gameObject, 5f);
        
        bulletSound.Play();
    }
    
    private void FixedUpdate()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if(owner == "enemy" && other.gameObject.CompareTag("Enemy")) return;
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && owner == "enemy") return;
        
        if (other.TryGetComponent(out LivingEntity livingEntity))
        {
            livingEntity.Damage(1);
            Destroy(gameObject);
        }
    }
}
