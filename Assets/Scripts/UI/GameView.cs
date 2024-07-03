
using System;
using System.Collections;
using System.Collections.Generic;
using MelenitasDev.SoundsGood;
using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UIElements;

public class GameView : View
{

    public UIDocument ui;
    public List<Sprite> healthList;
    public List<Sprite> bossHealthList;
    private VisualElement _overlay;
    private Label _sceneNameLabel;
    private Label _dialogueLabel;
    private Button _leftChoice;
    private Button _rightChoice;
    private VisualElement _healthBar;
    private VisualElement _bossHealthBar;

    private VisualElement _mistakeElement;
    private Label _mistakeLabel;

    private string _textToAnimate;


    private void OnEnable()
    {
        _overlay = ui.rootVisualElement.Q<VisualElement>("Overlay");
        _sceneNameLabel = ui.rootVisualElement.Q<Label>("Title");
        _dialogueLabel = ui.rootVisualElement.Q<Label>("Dialogue");
        _leftChoice = ui.rootVisualElement.Q<Button>("Left");
        _rightChoice = ui.rootVisualElement.Q<Button>("Right");
        _healthBar = ui.rootVisualElement.Q<VisualElement>("HealthBar");
        _bossHealthBar = ui.rootVisualElement.Q<VisualElement>("BossHealthBar");

        _mistakeElement = ui.rootVisualElement.Q<VisualElement>("MistakeTitle");
        _mistakeLabel = ui.rootVisualElement.Q<Label>("MistakeTitleLabel");
        
        _leftChoice.clicked += () => GameEvents.OnPlayerChooses.Invoke("left");
        _rightChoice.clicked += () => GameEvents.OnPlayerChooses.Invoke("right");

    }

    public override void InitializeView(UIController uiController)
    {
        GameEvents.OnChangeRoom.AddListener(ChangeRoom);
        GameEvents.OnEnteredScene.AddListener(ShowSceneName);
        
        GameEvents.ShowPlayerHealth.AddListener(ShowHealthBar);
        GameEvents.HidePlayerHealth.AddListener(HideHealthBar);
        GameEvents.UpdatePlayerHealth.AddListener(UpdateHealthBar);
        
        GameEvents.ShowBossHealth.AddListener(ShowBossHealthBar);
        GameEvents.HideBossHealth.AddListener(HideBossHealthBar);
        GameEvents.UpdateBossHealth.AddListener(UpdateBossHealthBar);
        
    }

    public override void UpdateView()
    {
        
    }

    public override void ExitView()
    {
        GameEvents.OnChangeRoom.RemoveListener(ChangeRoom);
        GameEvents.OnEnteredScene.RemoveListener(ShowSceneName);
        
        GameEvents.ShowPlayerHealth.RemoveListener(ShowHealthBar);
        GameEvents.HidePlayerHealth.RemoveListener(HideHealthBar);
        GameEvents.UpdatePlayerHealth.RemoveListener(UpdateHealthBar);
        
        GameEvents.ShowBossHealth.RemoveListener(ShowBossHealthBar);
        GameEvents.HideBossHealth.RemoveListener(HideBossHealthBar);
        GameEvents.UpdateBossHealth.RemoveListener(UpdateBossHealthBar);
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
    public void FadeIn()
    {
        _overlay.style.display = DisplayStyle.Flex;
        _overlay.AddToClassList("black");
    }
    
    public void FadeOut()
    {
        _overlay.RemoveFromClassList("black");
        Tween.Delay(0.5f, onComplete: () => _overlay.style.display = DisplayStyle.None);
    }

    public void PlayTextAnimation(string textToAnimate)
    {
        HideChoices();
        _dialogueLabel.style.display = DisplayStyle.Flex;
        _textToAnimate = textToAnimate;

        _dialogueLabel.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 1f));
        _dialogueLabel.text = "";
        _dialogueLabel.style.unityTextAlign = TextAnchor.UpperLeft;
        
        StartCoroutine(TypeWriterEffect());
        
    }

    public void HideDialogueBox()
    {
        _dialogueLabel.text = "";
        _dialogueLabel.style.display = DisplayStyle.None;
    }

    private IEnumerator TypeWriterEffect()
    {

        foreach (var letter in _textToAnimate.ToCharArray())
        {

            _dialogueLabel.text += letter;
            
            if (letter == ' ')
            {
                continue;
            }
            
            yield return new WaitForSeconds(0.025f);
            
        }
        
        GameEvents.OnUIDialogueFinishWriting.Invoke();
        
    }

    public void ShowChoices(string right, string left)
    {
        HideDialogueBox(); 
        ui.rootVisualElement.Q<VisualElement>("Choice").style.display = DisplayStyle.Flex;
        
        _rightChoice.text = right;
        _leftChoice.text = left;
    }
    
    public void HideChoices()
    {
        ui.rootVisualElement.Q<VisualElement>("Choice").style.display = DisplayStyle.None;
    }

    public void HideLeftButton()
    {
        ui.rootVisualElement.Q<Button>("Left").style.display = DisplayStyle.None;
    }
    
    public void ShowLeftButton()
    {
        ui.rootVisualElement.Q<Button>("Left").style.display = DisplayStyle.Flex;
    }

    public void ShowMistakeTitle(string text)
    {
        _mistakeLabel.text = text;
        _mistakeElement.AddToClassList("show-mistake-title");
    }
    
    public void HideMistakeTitle()
    {
        _mistakeElement.RemoveFromClassList("show-mistake-title");
    }
    
    public void ShowHealthBar()
    {
        _healthBar.style.display = DisplayStyle.Flex;
    }
    
    public void HideHealthBar()
    {
        _healthBar.style.display = DisplayStyle.None;
    }
    
    public void UpdateHealthBar(int health)
    {
        _healthBar.style.backgroundImage = new StyleBackground(healthList[health]);
    }
    
    public void ShowBossHealthBar()
    {
        _bossHealthBar.style.display = DisplayStyle.Flex;
    }
    
    public void HideBossHealthBar()
    {
        _bossHealthBar.style.display = DisplayStyle.None;
    }
    
    public void UpdateBossHealthBar(int health)
    {
        _bossHealthBar.style.backgroundImage = new StyleBackground(bossHealthList[health]);
    }
    
}
