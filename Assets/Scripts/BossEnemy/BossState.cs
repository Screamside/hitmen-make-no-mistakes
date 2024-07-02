using UnityEngine;

public abstract class BossState
{

    public abstract void EnterState(BossBehaviour enemyBehaviour);
    public abstract void UpdateState();
    public abstract void ExitState();
    
}
