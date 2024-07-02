using PrimeTween;
using UnityEngine;

public class BossShootingState : BossState
{
    public override void EnterState(BossBehaviour enemyBehaviour)
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
                        enemyBehaviour.SwitchState(BossStateType.MovingAway);
                    });
                });
            }
            else
            {
                Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
                {
                    enemyBehaviour.SwitchState(BossStateType.MovingAway);
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
