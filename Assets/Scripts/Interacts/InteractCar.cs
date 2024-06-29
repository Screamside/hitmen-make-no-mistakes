using UnityEngine;

public class InteractCar : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Car Interacted");
        CutsceneManager.PlayCutscene(2);
    }
}
