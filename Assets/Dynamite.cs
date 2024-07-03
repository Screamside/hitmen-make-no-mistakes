using System;
using MelenitasDev.SoundsGood;
using PrimeTween;
using UnityEngine;

public class Dynamite : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;

    public Vector2 direction;
    public float throwForce = 10f;
    public float maxLifetime = 2f;
    public SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;
    public float damage = 1;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, maxLifetime);
    }

    private void Start()
    {
        _rigidbody2D.velocity = direction.normalized * throwForce;
        _rigidbody2D.angularVelocity = 100f;
    }

    private void OnDestroy()
    {
        
        Physics2D.OverlapCircle(transform.position, 4, LayerMask.GetMask("Player")).GetComponent<PlayerHealth>().Damage(1);
        
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Tween.StopAll(spriteRenderer);
        new Sound(SFX.Explosion).SetSpatialSound(false).SetOutput(Output.SFX).Play();
    }

}
