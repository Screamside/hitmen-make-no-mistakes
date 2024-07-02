using System;
using UnityEngine;

public class Dynamite : MonoBehaviour
{

    private Rigidbody2D _rigidbody2D;

    public Vector2 direction;
    public float throwForce = 10f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    private void Start()
    {
        _rigidbody2D.velocity = direction.normalized * throwForce;
    }
}
