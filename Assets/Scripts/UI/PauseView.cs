using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseView : View
{

    public UIDocument ui;
    private Button _continueGame;
    private Button _resetGame;
    private Slider _musicSlider;
    private Slider _sfxSlider;

    private Label _mistake1;
    private Label _mistake2;
    private Label _mistake3;
    private Label _mistake4;
    private Label _mistake5;
    private Label _mistake6;
    private Label _mistake7;

    private void Awake()
    {
        _continueGame = ui.rootVisualElement.Q<Button>("ContinueGame");
        _resetGame = ui.rootVisualElement.Q<Button>("ResetGame");
        _musicSlider = ui.rootVisualElement.Q<Slider>("Music");
        _sfxSlider = ui.rootVisualElement.Q<Slider>("SFX");
        
        _mistake1 = ui.rootVisualElement.Q<Label>("Mistake1");
        _mistake2 = ui.rootVisualElement.Q<Label>("Mistake2");
        _mistake3 = ui.rootVisualElement.Q<Label>("Mistake3");
        _mistake4 = ui.rootVisualElement.Q<Label>("Mistake4");
        _mistake5 = ui.rootVisualElement.Q<Label>("Mistake5");
        _mistake6 = ui.rootVisualElement.Q<Label>("Mistake6");
        _mistake7 = ui.rootVisualElement.Q<Label>("Mistake7");
    }

    public override void InitializeView(UIController uiController)
    {

        _mistake1.text = "Mistake 1# - ???????????";
        _mistake2.text = "Mistake 2# - ???????????";
        _mistake3.text = "Mistake 3# - ???????????";
        _mistake4.text = "Mistake 4# - ???????????";
        _mistake5.text = "Mistake 5# - ???????????";
        _mistake6.text = "Mistake 6# - ???????????";
        _mistake7.text = "Mistake 7# - ???????????";
        
        if (GameManager.IsMistakeDone("ExitDoor"))
        {
            _mistake1.text = "Mistake 1# - Change of pace";
        }
        
        if (GameManager.IsMistakeDone("WrongDoor"))
        {
            _mistake1.text = "Mistake 2# - Oldest otaku";
        }
        
        if (GameManager.IsMistakeDone("BossMistake"))
        {
            _mistake1.text = "Mistake 3# - Not your area of expertise";
        }
        
        if (GameManager.IsMistakeDone("LostKeys"))
        {
            _mistake1.text = "Mistake 4# - Pesky rat";
        }
        
        if (GameManager.IsMistakeDone("Reception"))
        {
            _mistake1.text = "Mistake 5# - Where's the pizza?";
        }
        
        if (GameManager.IsMistakeDone("Lasers"))
        {
            _mistake1.text = "Mistake 6# - Dangerous lights show";
        }
        
        if (GameManager.IsMistakeDone("Caught"))
        {
            _mistake1.text = "Mistake 7# - Behind you!";
        }
    }

    public override void UpdateView()
    {
        
    }

    public override void ExitView()
    {
        
    }
}
