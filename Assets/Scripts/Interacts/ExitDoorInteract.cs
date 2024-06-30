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
            
            GameEvents.OnUIDialogueFinishWriting.AddListener(WaitForKeyPress);
        }
        else
        {
            CutsceneManager.PlayMistake("ExitDoor");
            GameManager.UpdateMistake("ExitDoor", true);
        }

        void WaitForKeyPress()
        {
            GameEvents.OnAnyKeyPress.AddListener(AfterInteract);
            GameEvents.OnUIDialogueFinishWriting.RemoveListener(WaitForKeyPress);
        }
        
    }

    public void AfterInteract()
    {
        GameEvents.OnAnyKeyPress.RemoveListener(AfterInteract);
        UIController.HideDialogue();
        GameManager.EnablePlayerControls();
    }
}
