using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = PrimeTween.Sequence;

public class BossSwingBatState : HostileEnemyState
{
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {

        Vector3 originalRotation = enemyBehaviour.weaponGameObject.transform.eulerAngles;
        Vector3 originalPosition = enemyBehaviour.weaponGameObject.transform.position;
        
        if(enemyBehaviour.transform.position.x > enemyBehaviour.player.transform.position.x)
        {
            Sequence.Create(Tween.Delay(enemyBehaviour.delayBeforeAttack))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.prepareBatAngle),
                    enemyBehaviour.prepareBatTime))
                .Group(Tween.Position(enemyBehaviour.weaponGameObject.transform, enemyBehaviour.prepareBatPosition.position, enemyBehaviour.prepareBatTime)
                    .OnComplete(() => enemyBehaviour.warning.SetActive(true)))
                .Chain(Tween.Color(enemyBehaviour.weaponSpriteRenderer, Color.red, enemyBehaviour.delaySwingBat/4, cycles: 4, cycleMode: CycleMode.Restart).OnComplete(() =>
                {
                    enemyBehaviour.weaponSpriteRenderer.color = Color.white;
                    enemyBehaviour.warning.SetActive(false);
                }))
                
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Group(Tween.Position(enemyBehaviour.weaponGameObject.transform, originalPosition, enemyBehaviour.swingBatTime))
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
                .Chain(Tween.Color(enemyBehaviour.weaponSpriteRenderer, Color.red, enemyBehaviour.delaySwingBat/4, cycles: 4, cycleMode: CycleMode.Restart).OnComplete(() => enemyBehaviour.weaponSpriteRenderer.color = Color.white))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Group(Tween.Position(enemyBehaviour.weaponGameObject.transform, originalPosition, enemyBehaviour.swingBatTime))
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
