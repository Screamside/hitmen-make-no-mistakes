using PrimeTween;
using UnityEngine;

public class BossShootingGunState : BossState
{
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        Tween.Delay(enemyBehaviour.delayBeforeShooting, () =>
        {
            enemyBehaviour.SpawnGunBullet();
            Tween.Delay(enemyBehaviour.delayAfterShooting, () =>
            {
                enemyBehaviour.SwitchToNextState();
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
