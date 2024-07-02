using PrimeTween;
using UnityEngine;

public class BossShootingSMGState : BossState
{
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        Tween.Delay(enemyBehaviour.delayBeforeShooting, () =>
        {
            enemyBehaviour.SpawnSMGBullet();
            Tween.Delay(enemyBehaviour.delayBetweenShots * 3).OnComplete(() =>
            {
                Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
                {
                    //enemyBehaviour.SwitchState(BossStateType.MovingAway);
                });
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
