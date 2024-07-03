using System;
using PrimeTween;
using UnityEngine;

public class EnemyRangeTrigger : MonoBehaviour
{

    public EnemyBehaviour EnemyBehaviour;
    
    public static bool triggered = false;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            EnemyBehaviour.canWalk = false;
            
            CutsceneManager.PlayMistake("Caught");
            EnemyBehaviour.enabled = false;
            
            Tween.Delay(3f, () =>
            {
                triggered = false;
                EnemyBehaviour.canWalk = true;
            });
            
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
