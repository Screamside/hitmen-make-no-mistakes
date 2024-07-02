using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{

    public int health;
    public bool invincibility;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = health;
    }

    public void Damage(int amount)
    {
        //TODO update UI
        if(invincibility) return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            GameManager.RestartFromMistake("DiedFromBullet");
        }

    }
    
}
