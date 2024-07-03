using System;
using Unity.Cinemachine;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    
    public CinemachineCamera cinemachineCamera;
    public string SceneName;
    public Soundtracks sceneSoundTrack;
    public bool forceReplay;

    public bool equipGun;

    public bool isBoss;
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            cinemachineCamera.gameObject.SetActive(true);
            CutsceneManager.Instance.currentCamera = cinemachineCamera;
            GameEvents.OnEnteredScene.Invoke(SceneName);
            GameManager.PlaySoundtrack(sceneSoundTrack, forceReplay);

            if (equipGun)
            {
                other.GetComponent<PlayerPistol>().enabled = true;
                GameEvents.ShowPlayerHealth.Invoke();
                GameEvents.UpdatePlayerHealth.Invoke(other.GetComponent<PlayerHealth>().health);

                
                
            }
            else
            {
                other.GetComponent<PlayerPistol>().enabled = false;
                GameEvents.HidePlayerHealth.Invoke();
            }
            
            if (isBoss)
            {
                GameEvents.ShowBossHealth.Invoke();
                GameEvents.UpdateBossHealth.Invoke(20);
            }
            else
            {
                GameEvents.HideBossHealth.Invoke();
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            cinemachineCamera.gameObject.SetActive(false);
        }
        
    }
}
