using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<String> mistakes;

    public PlayerController player;

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
    
}

