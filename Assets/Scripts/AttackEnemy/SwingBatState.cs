using PrimeTween;
using UnityEngine;

public class SwingBatState : HostileEnemyState
{
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {

        Vector3 originalRotation = enemyBehaviour.weaponGameObject.transform.eulerAngles;
        
        if(enemyBehaviour.transform.position.x > enemyBehaviour.player.transform.position.x)
        {
            Sequence.Create(Tween.Delay(enemyBehaviour.delayBeforeAttack))
                .Group(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.prepareBatAngle),
                    enemyBehaviour.prepareBatTime))
                .Group(Tween.Position(enemyBehaviour.weaponGameObject.transform, enemyBehaviour.prepareBatPosition.position, enemyBehaviour.prepareBatTime))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform, Quaternion.Euler(originalRotation), enemyBehaviour.resetBatRotationTime))
                .Chain(Tween.Delay(enemyBehaviour.delayAfterSwing))
                .OnComplete(() =>
                {
                    enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                });
        }
        else
        {
            Sequence.Create(Tween.Delay(enemyBehaviour.delayBeforeAttack))
                .Group(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * enemyBehaviour.prepareBatAngle),
                    enemyBehaviour.prepareBatTime))
                .Group(Tween.Position(enemyBehaviour.weaponGameObject.transform, enemyBehaviour.prepareBatPosition.position, enemyBehaviour.prepareBatTime))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform, Quaternion.Euler(originalRotation), enemyBehaviour.resetBatRotationTime))
                .Chain(Tween.Delay(enemyBehaviour.delayAfterSwing))
                .OnComplete(() =>
                {
                    enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                });
        }

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
