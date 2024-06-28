using UnityEngine;

public abstract class EnemyState
{

    public abstract void EnterState(EnemyBehaviour enemyBehaviour);
    public abstract void UpdateState();
    public abstract void ExitState();
    
}
