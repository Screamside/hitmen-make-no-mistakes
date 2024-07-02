using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;

public class BossMovingAwayState : BossState
{
    
    private BossBehaviour _hostileEnemyBehaviour;
    private Transform _transform;
    
    public override void EnterState(BossBehaviour hostileEnemyBehaviour)
    {
        _hostileEnemyBehaviour = hostileEnemyBehaviour;
        _transform = hostileEnemyBehaviour.transform;

        Vector3 finalPosition;
        
        if (_transform.position.x > hostileEnemyBehaviour.player.transform.position.x) // To the right of the player
        {

            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.right, hostileEnemyBehaviour.maxDistanceToRunAway, hostileEnemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.right * 0.5f) + (Vector2.down * 0.5f);
                Debug.DrawLine(hostileEnemyBehaviour.transform.position, finalPosition, Color.red);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.right * hostileEnemyBehaviour.maxDistanceToRunAway);
            }
            
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position + Vector3.up * 0.5f, Vector2.left, hostileEnemyBehaviour.maxDistanceToRunAway, hostileEnemyBehaviour.layermask);
            
            if (hit.collider != null)
            {
                finalPosition = hit.point + (Vector2.left * 0.5f) + (Vector2.down * 0.5f);
            }
            else
            {
                finalPosition = _transform.position + (Vector3.left * hostileEnemyBehaviour.maxDistanceToRunAway);
                Debug.Log("nowall");
            }
        }

        
        Tween.PositionAtSpeed(_hostileEnemyBehaviour.transform, finalPosition, hostileEnemyBehaviour.moveSpeed, ease: Ease.Linear)
            .OnComplete(() =>
            {
                _hostileEnemyBehaviour.SwitchState(BossStateType.MovingCloser);
                
            });

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
