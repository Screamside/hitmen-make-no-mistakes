using System;
using PrimeTween;
using UnityEngine;

public class GoBossInteraction : MonoBehaviour, IInteractable
{

    public Transform destination;

    public void Interact()
    {
        GameEvents.OnChangeRoom.Invoke();
        
        GameManager.UpdateCutscene("GoToBoss", true);

        Tween.Delay(0.5f, () =>
        {
            GameManager.Instance.player.PausePhysics();
            GameManager.Instance.player.transform.position = destination.position;
            GameManager.Instance.player.ResumePhysics();
        });
    }
}
