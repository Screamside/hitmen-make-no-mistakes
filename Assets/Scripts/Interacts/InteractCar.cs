using UnityEngine;

public class InteractCar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        if (GameManager.IsMistakeDone("LostKeys"))
        {
            CutsceneManager.PlayCutscene("ExitHeadquarters");
        }
        else
        {
            CutsceneManager.PlayMistake("LostKeys");
        }
        
    }
}
