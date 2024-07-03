using System;
using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{

    public EnemyBehaviour EnemyBehaviour;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CutsceneManager.PlayMistake("Caught");
            EnemyBehaviour.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EnemyBehaviour.enabled = true;
        }
    }
}
