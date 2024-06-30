using System;
using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using Unity.Cinemachine;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }
    public List<CutscenePlayer> cutscenes = new();
    public List<MyKeyPair<string, CutscenePlayer>> mistakes;
    public CinemachineCamera currentCamera;

    public int lastCutscene = 0;
    public string lastMistake;

    public bool wasLastAMistake;

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

        if (wasLastAMistake)
        {
            
            GameManager.RestartFromMistake(lastMistake);
            
        }
        else
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
        
        
    }

    public static void PlayCutscene(int index)
    {
        Instance.lastCutscene = index;
        
        UIController.FadeIn();
        Tween.Delay(0.5f, () =>
        {
            UIController.FadeOut();
            Instance.cutscenes[index].PlayCutscene();
            Instance.wasLastAMistake = false;

        });
    }

    public static void PlayMistake(string mistake)
    {
        if (Instance.mistakes.All(keyPair => keyPair.key != mistake))
        {
            Debug.LogError("No such mistake exists: " + mistake);
            return;
        }
        
        GameManager.DisablePlayerControls();
        
        var noise = Instance.currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        noise.enabled = true;

        Sequence.Create()
            .Chain(Tween.Delay(0.5f, () => noise.enabled = false))
            .Chain(Tween.Delay(2f, () => UIController.FadeIn()))
            .Chain(Tween.Delay(0.5f, () =>
            {
                UIController.FadeOut();

                Instance.mistakes.First(keyPair => keyPair.key == mistake).value.PlayCutscene();
                Instance.lastMistake = mistake;
                Instance.wasLastAMistake = true;
            }));
    }
}

