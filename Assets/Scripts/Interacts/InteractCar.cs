using UnityEngine;

public class InteractCar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (GameManager.IsMistakeDone("LostKeys"))
        {
            GameManager.DisablePlayerControls();
            CutsceneManager.PlayCutscene("ExitHeadquarters");
        }
        else
        {
            GameManager.DisablePlayerControls();
            CutsceneManager.PlayMistake("LostKeys");
        }
        
    }
}
