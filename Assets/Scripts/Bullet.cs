using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] private float speed = 5f;
    
    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    
    private void FixedUpdate()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
    
}
