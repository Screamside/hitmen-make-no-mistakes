using Unity.Cinemachine;
using UnityEngine;

public class BossInteraction : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        CutsceneManager.PlayCutscene(0);
    }
}
