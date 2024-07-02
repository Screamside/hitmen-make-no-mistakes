using System;
using System.Collections.Generic;
using System.Numerics;
using MelenitasDev.SoundsGood;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public PlayerController player;

    public Transform restartPoint;
    public Transform carParkPoint;

    private Soundtracks currentSoundtrack;
    private Sound playingSoundtrack;
    private Sound playingLoopedSoundtrack;
    
    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        
        foreach (var cinemachineCamera in FindObjectsByType<CinemachineCamera>(
                     FindObjectsSortMode.None))
        {
            cinemachineCamera.gameObject.SetActive(false);
        }

        InputSystem.onAnyButtonPress.CallOnce(HandleAnyKeyPress);
        
        void HandleAnyKeyPress(InputControl control)
        {
            if (!control.name.Equals("e"))
            {
                GameEvents.OnAnyKeyPress.Invoke();
            }
            InputSystem.onAnyButtonPress.CallOnce(HandleAnyKeyPress);
        }
        
        PlaySoundtrack(Soundtracks.Fight, true);
        
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public static void StopSoundtrack()
    {
        Instance.playingSoundtrack?.Stop();
        Instance.playingLoopedSoundtrack?.Stop();
    }
    
    public static void PlaySoundtrack(Soundtracks sound, bool force = false)
    {
        
        if (Instance.currentSoundtrack == sound && !force)
        {
            return;
        }

        Instance.currentSoundtrack = sound;
        
        StopSoundtrack();
        
        switch (sound)
        {
            case Soundtracks.Mistake:
                Instance.playingSoundtrack = new Sound(SFX.MistakeStart).SetOutput(Output.Music)
                    .OnComplete(() => Instance.PlayLoopedSoundtrack(SFX.Mistake));
                break;
            case Soundtracks.Fight:
                Instance.playingSoundtrack = new Sound(SFX.FightStart).SetOutput(Output.Music)
                    .OnComplete(() => Instance.PlayLoopedSoundtrack(SFX.Fight));
                break;
            case Soundtracks.Stealth:
                Instance.playingSoundtrack = new Sound(SFX.StealthStart).SetOutput(Output.Music)
                    .OnComplete(() => Instance.PlayLoopedSoundtrack(SFX.Stealth));
                break;
            case Soundtracks.Boss:
                Instance.playingSoundtrack = new Sound(SFX.BossStart).SetOutput(Output.Music)
                    .OnComplete(() => Instance.PlayLoopedSoundtrack(SFX.Boss));
                break;
            default:
                return;
        }
        
        Instance.playingSoundtrack.SetLoop(false).SetSpatialSound(false).Play();

    }

    private void PlayLoopedSoundtrack(SFX sfx)
    {
        Instance.playingLoopedSoundtrack = new Sound(sfx).SetLoop(true).SetSpatialSound(false).SetOutput(Output.Music);
        
        Instance.playingLoopedSoundtrack.Play();
    }

    public static bool IsCutsceneDone(string cutscene)
    { 
        return PlayerPrefs.GetInt(cutscene) == 1;
    }

    public static void UpdateCutscene(string cutscene, bool done)
    {
        PlayerPrefs.SetInt(cutscene, done ? 1 : 0);
    }
    
    public static bool IsMistakeDone(string mistake)
    { 
        return PlayerPrefs.GetInt(mistake) == 1;
    }

    public static void UpdateMistake(string mistake, bool done)
    {
        PlayerPrefs.SetInt(mistake, done ? 1 : 0);
    }

    public static void EnablePlayerControls()
    {
        Instance.player.EnableInput();
    }

    public static void DisablePlayerControls()
    {
        Instance.player.DisableInput();
    }

    public static void StartCarParkArea()
    {
        Instance.player.transform.position = Instance.carParkPoint.position;
        Instance.player.transform.localScale = Vector3.one;
        Instance.player.EnablePistol();
        
    }
    
    public static void RestartFromMistake(string mistake)
    {
        UIController.FadeIn();
        
        UIController.HideMistakeTitle();
        
        Tween.Delay(1f, () =>
        {
            foreach (var cinemachineCamera in FindObjectsByType<CinemachineCamera>(
                         FindObjectsSortMode.None))
            {
                cinemachineCamera.gameObject.SetActive(false);
            }
            UpdateMistake(mistake, true);
            Instance.player.transform.position = Instance.restartPoint.position;
            Instance.player.transform.rotation = Instance.restartPoint.rotation;
            Instance.player.transform.localScale = new Vector3(1f, 1f, 1f);
            Instance.player.DisablePistol();
            
            UIController.HideDialogue();
            UIController.HideChoices();
            UIController.FadeOut();
            switch (mistake)
            {
                case "ExitDoor":
                    Instance.ExitDoor();
                    break;
                
                case "WrongDoor":
                    Instance.WrongDoor();
                    break;
                
                case "BossMistake":
                    Instance.BossMistake();
                    break;
                
                case "LostKeys":
                    Instance.LostKeys();
                    break;
                
                case "Reception":
                    Instance.Reception();
                    break;
            }
        });
        
        GameEvents.OnUIDialogueFinishWriting.AddListener(WaitForInput);
        
        void WaitForInput()
        {
            GameEvents.OnUIDialogueFinishWriting.RemoveListener(WaitForInput);
            GameEvents.OnAnyKeyPress.AddListener(OnAnyKeyPress);
            
        }
        
        void OnAnyKeyPress()
        {
            GameEvents.OnAnyKeyPress.RemoveListener(OnAnyKeyPress);
            UIController.HideDialogue();
            EnablePlayerControls();
        }
        
    }

    private void ExitDoor()
    {
        Tween.Delay(2f, () =>
        {
            UIController.ShowDialogue("After checking your bank balance, you see enough money to live comfortably... until thursday.  \n\nWell, retirement sounds fun, but you need enough money for it.");
        });
    }
    
    private void WrongDoor()
    {
        Tween.Delay(2f, () =>
        {
            UIController.ShowDialogue("The boss gave you a considerable amount of money to keep your mouth shut.  \n\nThis was your most profitable service til today, but the trauma is not worth this kind of experience... ");
        });
    }

    private void BossMistake()
    {
        Tween.Delay(2f, () =>
        {
            UIController.ShowDialogue("What a truly crazy dream you had last night! \n\nYou recall something going wrong between you and your boss, but surely you're an indispensable asset to the organization... right?");
        });
    }
    
    private void LostKeys()
    {
        Tween.Delay(2f, () =>
        {
            UIController.ShowDialogue("You searched every nook and cranny of the garage but could not find the car keys... \n\nThankfully you had a spare set in your office.");
        });
    }
    
    private void Reception()
    {
        Tween.Delay(2f, () =>
        {
            UIController.ShowDialogue("After persuading them on telling that the pizza was in the car, you managed to escape. \n\nYou should take some precautions before performing this generic way of infiltrating...");
        });
    }
}

public enum Soundtracks
{
    Fight,
    Stealth,
    Boss,
    Mistake
}


