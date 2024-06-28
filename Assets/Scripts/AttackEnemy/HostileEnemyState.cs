using UnityEngine;

public abstract class HostileEnemyState
{

    public abstract void EnterState(HostileEnemyBehaviour enemyBehaviour);
    public abstract void UpdateState();
    public abstract void ExitState();
    
}
