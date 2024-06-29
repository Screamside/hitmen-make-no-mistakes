using System;
using PrimeTween;
using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{

    public Transform destination;

    public PlayerController player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
        }
    }

    public void Interact()
    {
        GameEvents.OnChangeRoom.Invoke();
        player.PausePhysics();

        Tween.Delay(duration: 0.5f, onComplete: () =>
        {
            player.transform.position = destination.position;
            player.ResumePhysics();
        });

    }
}
