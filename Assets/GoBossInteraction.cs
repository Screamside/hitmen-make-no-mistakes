using System;
using UnityEngine;

public class GoBossInteraction : MonoBehaviour
{

    public Transform destination;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            
            
        }
    }
}
