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

        float distanceToCheck = 0;

        if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex] == BossStateType.Swing)
        {
            distanceToCheck = _hostileEnemyBehaviour.minDistanceToSwing;
        }
        else
        {
            if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex-1] == BossStateType.ShootingSMG)
            {
                distanceToCheck = Random.Range(_hostileEnemyBehaviour.minDistanceToShootSMG.x,
                    _hostileEnemyBehaviour.minDistanceToShootSMG.y);
            }
        
            if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex] == BossStateType.ShootingGun)
            {
                distanceToCheck = Random.Range(_hostileEnemyBehaviour.minDistanceToShootGun.x,
                    _hostileEnemyBehaviour.minDistanceToShootGun.y);
            }
        }
        
        if (distance > distanceToCheck)
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

            _hostileEnemyBehaviour.SwitchToNextState();
            
        }
        
    }

    public override void ExitState()
    {
        
    }
}
