using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class MovingState : EnemyState
{
    
    private EnemyBehaviour _enemyBehaviour;

    private Transform _currentPoint;
    
    public override void EnterState(EnemyBehaviour enemyBehaviour)
    {
        _enemyBehaviour = enemyBehaviour;

        if (_enemyBehaviour.isStatic)
        {
            return;
        }
        
        if(_currentPoint == null)
        {
            _currentPoint = _enemyBehaviour.points[0];
            enemyBehaviour.transform.position = _currentPoint.position;
        }
        
        for (int i = 0; i < _enemyBehaviour.points.Count; i++)
        {
            if (_enemyBehaviour.points[i] == _currentPoint)
            {
                if (i == _enemyBehaviour.points.Count - 1)
                {
                    _currentPoint = _enemyBehaviour.points[0];
                }
                else
                {
                    _currentPoint = _enemyBehaviour.points[i + 1];
                }
                
                Tween.PositionAtSpeed(
                    enemyBehaviour.transform, 
                    _currentPoint.position, 
                    _enemyBehaviour.movementSpeed, 
                    ease: Ease.Linear).OnUpdate(enemyBehaviour.transform, (transform, tween) =>
                    {
                        if (!_enemyBehaviour.canWalk)
                        {
                            tween.Stop();
                            _enemyBehaviour.SwitchToNextStateInList();
                        }
                        
                    })
                    .OnComplete(() => _enemyBehaviour.SwitchToNextStateInList());
                
                break;
            }
        }
        
        _enemyBehaviour.animator.CrossFade("Walking", 0f);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
