using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

public class MovingAwayState : HostileEnemyState
{
    
    private HostileEnemyBehaviour _hostileEnemyBehaviour;
    private Transform _transform;
    
    public override void EnterState(HostileEnemyBehaviour hostileEnemyBehaviour)
    {
        if (hostileEnemyBehaviour.skipMoveAway)
        {
            _hostileEnemyBehaviour.SwitchState(HostileEnemyStateType.MovingCloser);
            return;
        }
        
        _hostileEnemyBehaviour = hostileEnemyBehaviour;
        _transform = hostileEnemyBehaviour.transform;

        Vector3 finalPosition;
        
        if (_transform.position.x > hostileEnemyBehaviour.player.transform.position.x) // To the right of the player
        {

            float movingAway = Random.Range(hostileEnemyBehaviour.maxDistanceToRunAway.x,
                hostileEnemyBehaviour.maxDistanceToRunAway.y);
            
            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.right, movingAway, hostileEnemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.right * 0.5f) + (Vector2.down * 0.5f);
                
                
                
                Debug.DrawLine(hostileEnemyBehaviour.transform.position, finalPosition, Color.red);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.right * movingAway);
            }
            
        }
        else
        {
            
            float movingAway = Random.Range(hostileEnemyBehaviour.maxDistanceToRunAway.x,
                hostileEnemyBehaviour.maxDistanceToRunAway.y);
            
            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.left, movingAway, hostileEnemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.left * 0.5f) + (Vector2.down * 0.5f);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.left * movingAway);
            }
        }
        
        
        Tween.PositionAtSpeed(_hostileEnemyBehaviour.transform, finalPosition, hostileEnemyBehaviour.moveSpeed, ease: Ease.Linear)
            .OnUpdate(_hostileEnemyBehaviour.transform, (transform, tween) =>
            {
                
                if (hostileEnemyBehaviour.skipMoveAway)
                {
                    _hostileEnemyBehaviour.SwitchState(HostileEnemyStateType.MovingCloser);
                    tween.Stop();
                }
                
            })
            .OnComplete(() =>
            {
                _hostileEnemyBehaviour.SwitchState(HostileEnemyStateType.MovingCloser);
                
            });

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
