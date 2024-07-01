using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    public static UIController Instance { get; private set; }

    public List<ViewReference> views = new();

    private View _currentView;
    public ViewType defaultView;

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
        
        views.ForEach(v => v.view.gameObject.SetActive(false));
        
        _currentView = views.Find( v => v.type == defaultView).view;
        _currentView.gameObject.SetActive(true);
        
        _currentView.InitializeView(Instance);
    }

    public void SwitchView(ViewType nextView)
    {
        
        _currentView.ExitView();
        _currentView.gameObject.SetActive(false);
        
        _currentView = views.Find( v => v.type == nextView).view;
        
        _currentView.gameObject.SetActive(true);
        _currentView.InitializeView(Instance);
        
    }
    
    public static void FadeIn()
    {
        if (Instance._currentView is GameView gameView)
        {
            gameView.FadeIn();
        }
    }
    
    public static void FadeOut()
    {
        if (Instance._currentView is GameView gameView)
        {
            gameView.FadeOut();
        }
    }

    public static void ShowChoices(string right, string left)
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.ShowChoices(right, left);
        }
    }
    
    public static void HideChoices()
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.HideChoices();
        }
    }

    public static void HideLeftButton()
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.HideLeftButton();
        }
    }
    
    public static void ShowLeftButton()
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.ShowLeftButton();
        }
    }
    public static void ShowDialogue(string textToAnimate)
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.PlayTextAnimation(textToAnimate);
        }
    }

    public static void HideDialogue()
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.HideDialogueBox();
        }
    }

    public static void ShowMistakeTitle(string text)
    {
        
        if(Instance._currentView is GameView gameView)
        {
            gameView.ShowMistakeTitle(text);
        }
        
    }

    public static void HideMistakeTitle()
    {
        if(Instance._currentView is GameView gameView)
        {
            gameView.HideMistakeTitle();
        }
    }
    
}

[Serializable]
public class ViewReference
{
    public View view;
    public ViewType type;
}

public enum ViewType
{
    GAME,
    PAUSE
}