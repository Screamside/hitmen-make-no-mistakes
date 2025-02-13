using System;
using MelenitasDev.SoundsGood;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dynamite : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;

    public Vector2 direction;
    public float throwForce = 10f;
    [MinMaxRangeSlider(0f, 10f)] public Vector2 maxLifetime;
    public SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;
    public float damage = 1;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(gameObject, Random.Range(maxLifetime.x, maxLifetime.y));
        ;
    }

    private void Start()
    {
        _rigidbody2D.linearVelocity = direction.normalized * throwForce;
        _rigidbody2D.angularVelocity = 100f;
    }

    private void OnDestroy()
    {
        
        var player = Physics2D.OverlapCircle(transform.position, 4, LayerMask.GetMask("Player"));

        if (playerHealth != null)
        {
            playerHealth.GetComponent<PlayerHealth>().Damage(1);
        }
        
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Tween.StopAll(spriteRenderer);
        new Sound(SFX.Explosion).SetSpatialSound(false).SetOutput(Output.SFX).Play();
    }

}
