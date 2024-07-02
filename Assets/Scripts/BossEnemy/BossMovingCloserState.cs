using UnityEngine;

public class BossMovingCloserState : BossState
{

    private BossBehaviour _hostileEnemyBehaviour;

    private Transform _transform;
    private Transform _playerTransform;
    
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        _hostileEnemyBehaviour = enemyBehaviour;
        _transform = enemyBehaviour.transform;
        _playerTransform = enemyBehaviour.player.transform;
    }

    public override void UpdateState()
    {
        
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);

        if (distance > _hostileEnemyBehaviour.minDistanceToShoot)
        {

            if (_transform.position.x > _playerTransform.position.x)
            {
                _transform.position += Vector3.left * (_hostileEnemyBehaviour.moveSpeed * Time.deltaTime);
            }
            else
            {
                _transform.position += Vector3.right * (_hostileEnemyBehaviour.moveSpeed * Time.deltaTime);
            }
            
        }
        else
        {

            
        }
        
    }

    public override void ExitState()
    {
        
    }
}
