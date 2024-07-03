using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovingAwayState : BossState
{
    
    private BossBehaviour _hostileEnemyBehaviour;
    private Transform _transform;
    
    public override void EnterState(BossBehaviour enemyBehaviour)
    {
        _hostileEnemyBehaviour = enemyBehaviour;
        _transform = enemyBehaviour.transform;

        Vector3 finalPosition;

        float distance = 0;
        
        
        if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex-1] == BossStateType.ShootingSMG)
        {
            distance = Random.Range(_hostileEnemyBehaviour.maxDistanceToRunAwaySMG.x,
                _hostileEnemyBehaviour.maxDistanceToRunAwaySMG.y);
        }
        
        if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex-1] == BossStateType.ShootingGun)
        {
            distance = Random.Range(_hostileEnemyBehaviour.maxDistanceToRunAwayGun.x,
                _hostileEnemyBehaviour.maxDistanceToRunAwayGun.y);
        }
        
        if (_hostileEnemyBehaviour.stateOrder[_hostileEnemyBehaviour.stateIndex-2] == BossStateType.Swing)
        {
            distance = Random.Range(_hostileEnemyBehaviour.maxDistanceToRunAwayBat.x, _hostileEnemyBehaviour.maxDistanceToRunAwayBat.y);
        }
        
        if (_transform.position.x > enemyBehaviour.player.transform.position.x) // To the right of the player
        {

            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.right, distance, enemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.right * 0.5f) + (Vector2.down * 0.5f);
                Debug.DrawLine(enemyBehaviour.transform.position, finalPosition, Color.red);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.right * distance);
            }
            
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.left, distance, enemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.left * 0.5f) + (Vector2.down * 0.5f);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.left * distance);
            }
        }

        
        Tween.PositionAtSpeed(_hostileEnemyBehaviour.transform, finalPosition, enemyBehaviour.moveSpeed, ease: Ease.Linear)
            .OnComplete(() =>
            {
                _hostileEnemyBehaviour.SwitchToNextState();
            });

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
