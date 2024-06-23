
using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UIElements;

public class GameView : View
{

    public UIDocument ui;
    private VisualElement _panel;
    private Label _sceneNameLabel;


    private void OnEnable()
    {
        _panel = ui.rootVisualElement.Q<VisualElement>("Panel");
        _sceneNameLabel = ui.rootVisualElement.Q<Label>("Title");
    }

    public override void InitializeView(UIController uiController)
    {
        
        GameEvents.OnChangeRoom.AddListener(ChangeRoom);
        GameEvents.OnEnteredScene.AddListener(ShowSceneName);
        
    }

    public override void UpdateView()
    {
        
    }

    public override void ExitView()
    {
        GameEvents.OnChangeRoom.RemoveListener(ChangeRoom);
        GameEvents.OnEnteredScene.RemoveListener(ShowSceneName);
    }

    private void ChangeRoom()
    {
        FadeIn();
        Tween.Delay(duration: 1f, onComplete: () => FadeOut());
    }

    private void ShowSceneName(string sceneName)
    {
        _sceneNameLabel.text = sceneName;
        Tween.Delay(duration: 0.5f, onComplete: () =>
        {
            _sceneNameLabel.style.color = new StyleColor(new Color(1, 1, 1, 1));
            
            Tween.Delay(duration:3f, onComplete: () =>
            {
                _sceneNameLabel.style.color = new StyleColor(new Color(1, 1, 1, 0));
            });
            
        });
    }
    private void FadeIn()
    {
        _panel.RemoveFromClassList("fade-out");
    }
    
    private void FadeOut()
    {
        _panel.AddToClassList("fade-out");
    }
    
}
