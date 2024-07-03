using System;
using PrimeTween;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{

    public Transform destination;

    public void Interact()
    {
        UIController.HideDialogue();
        GameEvents.OnChangeRoom.Invoke();
        GameManager.Instance.player.PausePhysics();

        Tween.Delay(duration: 0.5f, onComplete: () =>
        {
            GameManager.Instance.player.transform.position = destination.position;
            GameManager.Instance.player.ResumePhysics();
        });

    }
}
