using UnityEngine;

public abstract class BossState
{

    public abstract void EnterState(HostileEnemyBehaviour enemyBehaviour);
    public abstract void UpdateState();
    public abstract void ExitState();
    
}
