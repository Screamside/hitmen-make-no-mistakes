using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActivator : MonoBehaviour
{
    
    public List<GameObject> enemies;
    
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
                
            }
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
                
            }
        }
    }
}
