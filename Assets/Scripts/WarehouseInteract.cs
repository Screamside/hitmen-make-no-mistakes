using PrimeTween;
using UnityEngine;

public class WarehouseInteract : MonoBehaviour, IInteractable
{
    public Transform destination;
    
    public void Interact()
    {
        
        UIController.FadeIn();
        Tween.Delay(1f).OnComplete(() =>
        {
            GameManager.Instance.player.GetComponent<PlayerPistol>().enabled = false;
            GameEvents.ShowOverlayText.Invoke(
                "Unfortunately, due to your carelessness, you used up all of your ammo... \n\nLooks like you'll need to stealth your way through the enemies this time.");
        });
        GameManager.DisablePlayerControls();
        
        Tween.Delay(5f).OnComplete(() =>
        {
            GameEvents.OnAnyKeyPress.AddListener(OnAnyKeyPress);
                    
            void OnAnyKeyPress()
            {
                GameEvents.OnAnyKeyPress.RemoveListener(OnAnyKeyPress);
                
                UIController.HideDialogue();
                GameEvents.HideOverlayText.Invoke();
                GameManager.Instance.player.PausePhysics();
                UIController.FadeOut();
            
                GameManager.Instance.player.transform.position = destination.position;
                GameManager.Instance.player.ResumePhysics();
                GameManager.EnablePlayerControls();
            
            }
            
        });
        
        
        
        
        

    }
    
}
