using PrimeTween;
using UnityEngine;

public class BossShootingSMGState : BossState
{
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        Tween.Delay(enemyBehaviour.delayBeforeShooting, () =>
        {
            ;
            enemyBehaviour.SpawnSMGBullet();
            Tween.Delay(enemyBehaviour.delayBetweenShots * 5).OnComplete(() =>
            {
                Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
                {
                    enemyBehaviour.SwitchToNextState();
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
