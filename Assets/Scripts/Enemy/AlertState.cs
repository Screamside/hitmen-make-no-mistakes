using UnityEngine;

public class AlertState : EnemyState
{

    private EnemyBehaviour _enemyBehaviour;
    private float currentTimer;
    
    public override void EnterState(EnemyBehaviour enemyBehaviour)
    {
        _enemyBehaviour = enemyBehaviour;
        
        if(_enemyBehaviour.randomizeTimer)
        {
            currentTimer = Random.Range(_enemyBehaviour.randomizeRange.x, _enemyBehaviour.randomizeRange.y+1);
        }
        else
        {
            currentTimer = _enemyBehaviour.alertTimer;
        }
        
        _enemyBehaviour.animator.CrossFade("Idle", 0f);
        
    }

    public override void UpdateState()
    {

        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            _enemyBehaviour.SwitchToNextStateInList();
        }

    }

    public override void ExitState()
    {
        
    }
}
