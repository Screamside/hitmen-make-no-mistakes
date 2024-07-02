using UnityEngine;

public class BossDynamiteState : BossState
{
    
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        Debug.Log("THROW DYNAMITE");
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
