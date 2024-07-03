using PrimeTween;
using UnityEngine;

public class BossDynamiteState : BossState
{
    
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        Debug.Log("THROW DYNAMITE");

        Tween.Delay(enemyBehaviour.delayBeforeThrow, () =>
        {
            enemyBehaviour.SpawnDynamite();
            Tween.Delay(enemyBehaviour.delayAfterThrow, () => enemyBehaviour.SwitchToNextState());
        });

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
