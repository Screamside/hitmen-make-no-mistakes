using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{

    public GameObject door;
    public List<GameObject> enemies;
    private int enemiesCount;

    private void Awake()
    {
        enemiesCount = enemies.Count;
        
        foreach (var enemy in enemies)
        {
                
            if(enemy == null)
            {
                continue;
            }
                
            enemy.GetComponent<LivingEntity>().enemyActivator = this;

        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                
                if(enemy == null)
                {
                    continue;
                }
                
                HostileEnemyBehaviour enemyBehaviour = enemy.GetComponent<HostileEnemyBehaviour>();
            
                enemyBehaviour.enabled = true;
                enemyBehaviour.player = other.GetComponent<PlayerController>();
                door.SetActive(false);
                
            }
        }
    }

    public void EnemyDied()
    {
        enemiesCount--;

        if (enemiesCount <= 0)
        {
            door.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            foreach (var enemy in enemies)
            {
                
                if(enemy == null)
                {
                    continue;
                }
                
                HostileEnemyBehaviour enemyBehaviour = enemy.GetComponent<HostileEnemyBehaviour>();
                enemyBehaviour.enabled = false;
                enemy.GetComponent<LivingEntity>().enemyActivator = this;

            }
        }
    }
}
