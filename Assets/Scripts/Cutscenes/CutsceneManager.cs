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
    public List<MyKeyPair<string, CutscenePlayer>> cutscenes = new();
    public List<MyKeyPair<string, CutscenePlayer>> mistakes;
    public CinemachineCamera currentCamera;

    public string lastCutscene;
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
            UIController.FadeIn();

            Tween.Delay(0.5f, () =>
            {
                
                foreach (var cinemachineCamera in FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None))
                {
                    cinemachineCamera.gameObject.SetActive(false);
                }
        
                UIController.HideChoices();
                UIController.HideDialogue();
                UIController.HideMistakeTitle();
        
                currentCamera.gameObject.SetActive(true);

                switch (lastCutscene)
                {
                    case "ChooseReceptionPark":
                        GameManager.StartCarParkArea();
                        break;
                }
                
                GameManager.UpdateCutscene(lastCutscene, true);
                Tween.Delay(0.5f, () => UIController.FadeOut());
                
                GameManager.EnablePlayerControls();
            });
            
        }
    }

    public static void PlayCutscene(string cutscene)
    {
        if (Instance.cutscenes.All(keyPair => keyPair.key != cutscene))
        {
            Debug.LogError("No such cutscene exists: " + cutscene);
            return;
        }
        
        GameManager.DisablePlayerControls();
        
        Sequence.Create()
            .Chain(Tween.Delay(0.01f, () => UIController.FadeIn()))
            .Chain(Tween.Delay(0.5f, () =>
            {
                UIController.FadeOut();

                Instance.cutscenes.First(keyPair => keyPair.key == cutscene).value.PlayCutscene();
                Instance.lastCutscene = cutscene;
                Instance.wasLastAMistake = false;
            }));
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

