using Unity.Cinemachine;
using UnityEngine;

public class BossInteraction : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        if (GameManager.IsCutsceneDone("BossMission"))
        {
            GameManager.DisablePlayerControls();
            UIController.ShowDialogue("Why are you still here? Go on, I already told you what to do!");
            
            GameEvents.OnUIDialogueFinishWriting.AddListener(WaitForKeyPress);
        }
        else
        {
            CutsceneManager.PlayCutscene("BossMission");
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
