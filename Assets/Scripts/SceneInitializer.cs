using System;
using Unity.Cinemachine;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    
    public CinemachineCamera cinemachineCamera;
    public string SceneName;
    public Soundtracks sceneSoundTrack;
    public bool forceReplay;
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            cinemachineCamera.gameObject.SetActive(true);
            CutsceneManager.Instance.currentCamera = cinemachineCamera;
            GameEvents.OnEnteredScene.Invoke(SceneName);
            GameManager.PlaySoundtrack(sceneSoundTrack, forceReplay);
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
