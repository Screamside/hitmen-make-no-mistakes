using UnityEngine;

public class MistakeInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (GameManager.IsMistakeDone("WrongDoor"))
        {
            GameManager.DisablePlayerControls();
            UIController.ShowDialogue("I'm never entering that room again...");
            
            GameEvents.OnUIDialogueFinishWriting.AddListener(WaitForKeyPress);
        }
        else
        {
            CutsceneManager.PlayMistake("WrongDoor");
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
