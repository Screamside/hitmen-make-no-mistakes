using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = PrimeTween.Sequence;

public class BossSwingBatState : BossState
{
    public override void EnterState(BossBehaviour enemyBehaviour)
    {

        Vector3 originalRotation = enemyBehaviour.batObject.transform.eulerAngles;
        Vector3 originalPosition = enemyBehaviour.batObject.transform.position;
        
        if(enemyBehaviour.transform.position.x > enemyBehaviour.player.transform.position.x)
        {
            Sequence.Create(Tween.Delay(enemyBehaviour.delayBeforeAttack))
                .Chain(Tween.Rotation(enemyBehaviour.batObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.prepareBatAngle),
                    enemyBehaviour.prepareBatTime))
                .Group(Tween.Position(enemyBehaviour.batObject.transform, enemyBehaviour.prepareBatPosition.position, enemyBehaviour.prepareBatTime)
                    .OnComplete(() => enemyBehaviour.warning.SetActive(true)))
                .Chain(Tween.Color(enemyBehaviour.weaponSpriteRenderer, Color.red, enemyBehaviour.delaySwingBat/4, cycles: 4, cycleMode: CycleMode.Restart).OnComplete(() =>
                {
                    enemyBehaviour.weaponSpriteRenderer.color = Color.white;
                    enemyBehaviour.batObject.GetComponent<BoxCollider2D>().enabled = true;
                    enemyBehaviour.warning.SetActive(false);
                }))
                .Chain(Tween.Rotation(enemyBehaviour.batObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Group(Tween.Position(enemyBehaviour.batObject.transform, originalPosition, enemyBehaviour.swingBatTime))
                .Chain(Tween.Rotation(enemyBehaviour.batObject.transform, Quaternion.Euler(originalRotation), enemyBehaviour.resetBatRotationTime))
                .Chain(Tween.Delay(enemyBehaviour.delayAfterSwing))
                .OnComplete(enemyBehaviour.SwitchToNextState);
        }
        else
        {
            Sequence.Create(Tween.Delay(enemyBehaviour.delayBeforeAttack))
                .Group(Tween.Rotation(enemyBehaviour.batObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * enemyBehaviour.prepareBatAngle),
                    enemyBehaviour.prepareBatTime))
                .Group(Tween.Position(enemyBehaviour.batObject.transform, enemyBehaviour.prepareBatPosition.position, enemyBehaviour.prepareBatTime))
                .Chain(Tween.Color(enemyBehaviour.weaponSpriteRenderer, Color.red, enemyBehaviour.delaySwingBat/4, cycles: 4, cycleMode: CycleMode.Restart).OnComplete(() =>
                    {
                        enemyBehaviour.weaponSpriteRenderer.color = Color.white;
                        enemyBehaviour.batObject.GetComponent<BoxCollider2D>().enabled = true;
                        enemyBehaviour.warning.SetActive(false);
                    }))
                .Chain(Tween.Rotation(enemyBehaviour.batObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -enemyBehaviour.swingBatAngle), enemyBehaviour.swingBatTime))
                .Group(Tween.Position(enemyBehaviour.batObject.transform, originalPosition, enemyBehaviour.swingBatTime)
                    .OnComplete(() =>
                    {
                        enemyBehaviour.batObject.GetComponent<BoxCollider2D>().enabled = false;
                    }))
                .Chain(Tween.Rotation(enemyBehaviour.batObject.transform, Quaternion.Euler(originalRotation), enemyBehaviour.resetBatRotationTime))
                .Chain(Tween.Delay(enemyBehaviour.delayAfterSwing))
                .OnComplete(enemyBehaviour.SwitchToNextState);
        }

    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
