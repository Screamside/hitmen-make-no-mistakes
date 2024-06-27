using System;
using System.Collections.Generic;
using PrimeTween;
using Unity.Cinemachine;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }
    public List<CutscenePlayer> cutscenes = new();
    public List<CutscenePlayer> mistakes = new();
    public CinemachineCamera currentCamera;

    public bool isPlayingCutscene;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        GameEvents.OnCutsceneFinished.AddListener(ResetCamera);
        
    }

    private void ResetCamera()
    {
        
        foreach (var cinemachineCamera in GameObject.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            cinemachineCamera.gameObject.SetActive(false);
        }
        
        currentCamera.gameObject.SetActive(true);
        UIController.HideChoices();
        UIController.HideDialogue();
    }

    public static void PlayCutscene(int index)
    {
        UIController.FadeIn();
        Tween.Delay(0.5f, () =>
        {
            UIController.FadeOut();
            Instance.cutscenes[index].PlayCutscene();
            
        });
    }

    public static void PlayMistake(int index)
    {
        UIController.FadeIn();
        Tween.Delay(0.5f, () =>
        {
            UIController.FadeOut();
            Instance.cutscenes[index].PlayCutscene();
            
        });
    }
}
