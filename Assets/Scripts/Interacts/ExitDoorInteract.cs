using UnityEngine;

public class ExitDoorInteract : MonoBehaviour, IInteractable
{
    public void Interact()
    {

        if (GameManager.IsMistakeDone("ExitDoor"))
        {
            //TODO: say that you should really talk to the boss
        }
        else
        {
            CutsceneManager.PlayMistake("ExitDoor");
            GameManager.UpdateMistake("ExitDoor", true);
        }
        
    }
}
