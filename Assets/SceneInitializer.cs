using System;
using Unity.Cinemachine;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    
    public CinemachineCamera cinemachineCamera;
    public string SceneName;
    
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            cinemachineCamera.gameObject.SetActive(true);
            GameEvents.OnEnteredScene.Invoke(SceneName);
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
