using System;
using UnityEngine;

public class Bat : MonoBehaviour
{

    public int damage = 1;

    private void OnDisable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
