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

    public int lastCutscene = 0;
    public int lastMistake = 0;

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
        
        GameEvents.OnCutsceneFinished.AddListener(Handle);
        
    }

    private void Handle()
    {
        
        foreach (var cinemachineCamera in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
        {
            cinemachineCamera.gameObject.SetActive(false);
        }
        
        UIController.HideChoices();
        UIController.HideDialogue();
        UIController.HideMistakeTitle();

        switch (lastCutscene)
        {
            case 2:
                PlayCutscene(3);
                return;
        }
        
        currentCamera.gameObject.SetActive(true);
    }

    public static void PlayCutscene(int index)
    {
        Instance.lastCutscene = index;
        
        UIController.FadeIn();
        Tween.Delay(0.5f, () =>
        {
            UIController.FadeOut();
            Instance.cutscenes[index].PlayCutscene();
            
        });
    }

    public static void PlayMistake(string mistake)
    {
        UIController.FadeIn();
        Tween.Delay(0.5f, () =>
        {
            UIController.FadeOut();

            switch (mistake)
            {
                case "ExitDoor":
                    Instance.mistakes[1].PlayCutscene();
                    break;
            }
            
        });
    }
}
