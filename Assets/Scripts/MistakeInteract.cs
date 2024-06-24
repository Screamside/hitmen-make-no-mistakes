using UnityEngine;

public class MistakeInteract : MonoBehaviour, IInteractable
{

    public int mistakeId;
    
    public void Interact()
    {
        GameEvents.OnPlayerMakesMistake.Invoke(mistakeId);
    }
}
