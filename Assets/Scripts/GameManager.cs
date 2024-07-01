using System;
using System.Collections.Generic;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    public PlayerController player;

    public Transform restartPoint;

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
        
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
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
            Instance.player.GetComponent<PlayerPistol>().enabled = false;
            
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
            UIController.ShowDialogue("After checking your bank balance, you had enough money to live comfortably... until thursday.  \n\nWell retirement looked good, but you need enough money for it.");
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
            UIController.ShowDialogue("You searched every nook and cranny of the gare but could not find the car keys... \n\nThankfully you had a spare set in your office.");
        });
    }
}


