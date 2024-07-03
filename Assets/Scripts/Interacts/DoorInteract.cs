using System;
using PrimeTween;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{

    public Transform destination;

    public void Interact()
    {
        
        UIController.FadeIn();
        
        UIController.ShowDialogue("Unfortunately, due to your carelessness, you used up all of your ammo... \n\nLooks like you'll need to stealth your way through the enemies this time.");
        Tween.Delay(4f).OnComplete(() =>
        {
            UIController.HideDialogue();
            GameEvents.OnChangeRoom.Invoke();
            GameManager.Instance.player.PausePhysics();

            Tween.Delay(duration: 0.5f, onComplete: () =>
            {
                GameManager.Instance.player.transform.position = destination.position;
                GameManager.Instance.player.ResumePhysics();
            });
        });

    }
}
