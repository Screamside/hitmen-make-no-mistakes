
using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.UIElements;

public class GameView : View
{

    public UIDocument ui;
    private VisualElement _overlay;
    private Label _sceneNameLabel;
    private Label _dialogueLabel;

    private string _textToAnimate;

    private void OnEnable()
    {
        _overlay = ui.rootVisualElement.Q<VisualElement>("Overlay");
        _sceneNameLabel = ui.rootVisualElement.Q<Label>("Title");
        _dialogueLabel = ui.rootVisualElement.Q<Label>("Dialogue");
    }

    public override void InitializeView(UIController uiController)
    {
        
        GameEvents.OnChangeRoom.AddListener(ChangeRoom);
        GameEvents.OnEnteredScene.AddListener(ShowSceneName);
        GameEvents.AskForFadeIn.AddListener(FadeIn);
        GameEvents.AskForFadeOut.AddListener(FadeOut);
        GameEvents.AskForDialogueAnimation.AddListener(PlayTextAnimation);

    }

    public override void UpdateView()
    {
        
    }

    public override void ExitView()
    {
        GameEvents.OnChangeRoom.RemoveListener(ChangeRoom);
        GameEvents.OnEnteredScene.RemoveListener(ShowSceneName);
        GameEvents.AskForFadeIn.RemoveListener(FadeIn);
        GameEvents.AskForFadeOut.RemoveListener(FadeOut);
        GameEvents.AskForDialogueAnimation.RemoveListener(PlayTextAnimation);
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
        _overlay.AddToClassList("black");
    }
    
    private void FadeOut()
    {
        _overlay.RemoveFromClassList("black");
    }

    private void PlayTextAnimation(string textToAnimate)
    {
        _textToAnimate = textToAnimate;

        _dialogueLabel.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 1f));
        _dialogueLabel.text = "";
        _dialogueLabel.style.unityTextAlign = TextAnchor.UpperLeft;
        
        StartCoroutine(TypeWriterEffect());
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
        
    }
    
}
