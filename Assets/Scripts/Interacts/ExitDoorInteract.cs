using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class ExitDoorInteract : MonoBehaviour, IInteractable
{

    public Transform destination;
    
    public void Interact()
    {

        if (GameManager.IsCutsceneDone("ChooseReceptionPark"))
        {
            GameManager.StartCarParkArea();
            return;
        }
        
        if (GameManager.IsCutsceneDone("BossMission"))
        {
            GameEvents.OnChangeRoom.Invoke();
            
            Tween.Delay(0.5f, () =>
            {
                GameManager.Instance.player.transform.position = destination.position;
            });
            
            return;
        }
        
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
