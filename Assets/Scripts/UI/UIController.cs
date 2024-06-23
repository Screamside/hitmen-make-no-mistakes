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