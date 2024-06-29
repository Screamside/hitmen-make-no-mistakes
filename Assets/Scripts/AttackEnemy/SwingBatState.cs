using PrimeTween;
using UnityEngine;

public class SwingBatState : HostileEnemyState
{
    public override void EnterState(HostileEnemyBehaviour enemyBehaviour)
    {

        Vector3 originalRotation = enemyBehaviour.weaponGameObject.transform.eulerAngles;

        enemyBehaviour.canRotateWeapon = false;

        Sequence.Create(Tween.Delay(1f))
            .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                Quaternion.Euler(originalRotation + Vector3.forward * 20),
                0.1f))
            .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                Quaternion.Euler(originalRotation + Vector3.forward * -80f), 0.1f))
            .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform, Quaternion.Euler(originalRotation), 0.3f))
            .Chain(Tween.Delay(2f))
            .OnComplete(() =>
            {
                enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                enemyBehaviour.canRotateWeapon = true;
            });
        
        if(enemyBehaviour.transform.position.x > enemyBehaviour.player.transform.position.x)
        {
            Sequence.Create(Tween.Delay(1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * 20),
                    0.1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -80f), 0.1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform, Quaternion.Euler(originalRotation), 0.3f))
                .Chain(Tween.Delay(2f))
                .OnComplete(() =>
                {
                    enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                    enemyBehaviour.canRotateWeapon = true;
                });
        }
        else
        {
            Sequence.Create(Tween.Delay(1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * -20),
                    0.1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform,
                    Quaternion.Euler(originalRotation + Vector3.forward * 80f), 0.1f))
                .Chain(Tween.Rotation(enemyBehaviour.weaponGameObject.transform, Quaternion.Euler(originalRotation), 0.3f))
                .Chain(Tween.Delay(2f))
                .OnComplete(() =>
                {
                    enemyBehaviour.SwitchState(HostileEnemyStateType.MovingAway);
                    enemyBehaviour.canRotateWeapon = true;
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
