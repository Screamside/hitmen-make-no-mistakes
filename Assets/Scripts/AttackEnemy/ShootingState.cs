using PrimeTween;
using UnityEngine;

public class ShootingState : HostileEnemyState
{
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {
        Tween.Delay(enemyBehaviour.delayBeforeShooting, () =>
        {
            enemyBehaviour.SpawnBullet();
            if (enemyBehaviour.smgMan)
            {
                Tween.Delay(enemyBehaviour.delayBetweenShots * 3).OnComplete(() =>
                {
                    Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
                    {
                        enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                    });
                });
            }
            else
            {
                Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
                {
                    enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                });
            }
        });
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
