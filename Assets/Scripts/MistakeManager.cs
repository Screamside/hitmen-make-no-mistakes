using System;
using System.Collections.Generic;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

public class MistakeManager : MonoBehaviour
{
    
    public static MistakeManager Instance { get; private set; }

    public List<Mistake> mistakes = new();

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
    }

    public CinemachineCamera currentCamera;

    private void OnEnable()
    {
        GameEvents.OnPlayerMakesMistake.AddListener(OnPlayerMakesMistake);
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerMakesMistake.RemoveListener(OnPlayerMakesMistake);
    }

    private void OnPlayerMakesMistake(int i)
    {

        CinemachineBasicMultiChannelPerlin noise = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        noise.enabled = true;

        Sequence
            .Create(Tween.Custom(0, 0.5f, duration: 0.25f, onValueChange: f =>
            {
                noise.AmplitudeGain = f;
            }))
            .Chain(Tween.Custom(0.5f, 0, duration: 0.25f, onValueChange: f =>
            {
                noise.AmplitudeGain = f;
            }))
            .Chain(Tween.Delay(3f, onComplete: () =>
            {
                GameEvents.AskForFadeIn.Invoke();
            }))
            .Chain(Tween.Delay(1f, onComplete: () =>
            {
                currentCamera.gameObject.SetActive(false);
                mistakes[i].cutscene.gameObject.SetActive(true);
                mistakes[i].cutscene.Play();
                GameEvents.AskForFadeOut.Invoke();
                GameEvents.AskForDialogueAnimation.Invoke("In a classic display of overconfidence, you waltzed in despite the \"Do Not Enter\" sign, thinking it did't apply to your expert self. \n"
                                                          + "Little did you know, you had just entered the Boss's most heavily armed room. \n"
                                                          + "Naturally, the Boss wouldn't let this slide. Sometimes, even hitmen should rethink their sense of authority.\n");
            }));
        
    }
}
