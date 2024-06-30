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

    public List<String> mistakes;

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
        
        LoadMistakes();

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

    private void LoadMistakes()
    {
        foreach (var mistake in mistakes)
        {

            if (PlayerPrefs.HasKey(mistake))
            {
                continue;
            }
            
            PlayerPrefs.SetInt(mistake, 0);
        }
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
            
            UIController.HideDialogue();
            UIController.HideChoices();
            UIController.FadeOut();
            switch (mistake)
            {
                case "ExitDoor":
                    Instance.ExitDoor();
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
        }
        
    }

    private void ExitDoor()
    {
        UIController.ShowDialogue("After checking your bank balance, you had enough money to live comfortably... until thursday.  \n\nWell retirement looked good, but you need enough money for it.");
    }
    
}


