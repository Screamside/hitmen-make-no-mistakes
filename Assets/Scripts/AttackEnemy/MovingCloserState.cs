using UnityEngine;

public class MovingCloserState : HostileEnemyState
{

    private HostileEnemyBehaviour _hostileEnemyBehaviour;

    private Transform _transform;
    private Transform _playerTransform;

    private float randomDistance;
    
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {
        _hostileEnemyBehaviour = enemyBehaviour;
        _transform = enemyBehaviour.transform;
        _playerTransform = enemyBehaviour.player.transform;
        
        randomDistance = Random.Range(enemyBehaviour.minDistanceToShoot.x, enemyBehaviour.minDistanceToShoot.y);
    }

    public override void UpdateState()
    {
        
        float distance = Vector3.Distance(_transform.position, _playerTransform.position);

        if (distance > randomDistance)
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

            if (_hostileEnemyBehaviour.batMan)
            {
                _hostileEnemyBehaviour.SwitchState(HostileEnemyStateType.Swing);
            }
            else
            {
                _hostileEnemyBehaviour.SwitchState(HostileEnemyStateType.Shooting);
            }
        }
        
    }

    public override void ExitState()
    {
        
    }
}
