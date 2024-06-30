using UnityEngine;
using UnityEngine.InputSystem;

public class ExitDoorInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {

        if (GameManager.IsMistakeDone("ExitDoor"))
        {
            //TODO: say that you should really talk to the boss
            GameManager.DisablePlayerControls();
            UIController.ShowDialogue("I should really talk to the boss.");
            GameEvents.OnPlayerKeyPressAfterDialogue.AddListener(AfterInteract);
        }
        else
        {
            CutsceneManager.PlayMistake("ExitDoor");
            GameManager.UpdateMistake("ExitDoor", true);
        }
        
    }

    public void AfterInteract()
    {
        GameManager.EnablePlayerControls();
        GameEvents.OnPlayerKeyPressAfterDialogue.RemoveListener(AfterInteract);
        UIController.HideDialogue();
    }
}
