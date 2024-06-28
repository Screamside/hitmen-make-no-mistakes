using PrimeTween;
using UnityEngine;

public class ShootingState : HostileEnemyState
{
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {
        Tween.Delay(enemyBehaviour.delayBeforeShooting, () =>
        {
            Debug.Log("I SHOOTED");
            Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
            {
                enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
            });
        });
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
