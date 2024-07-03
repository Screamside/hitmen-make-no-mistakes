public static class GameEvents
{

    public static readonly GEvent<IInteractable> OnHoverInteractable = new();
    public static readonly GEvent<IInteractable> OnUnhoverInteractable = new();
    
    public static readonly GEvent OnChangeRoom = new();
    public static readonly GEvent<string> OnEnteredScene = new();

    public static readonly GEvent<int> OnPlayerMakesMistake = new();
    public static readonly GEvent<string> OnPlayerChooses = new();
    public static readonly GEvent OnAnyKeyPress = new();
    public static readonly GEvent OnUIDialogueFinishWriting = new();
    public static readonly GEvent ShowPlayerHealth = new();
    public static readonly GEvent HidePlayerHealth = new();
    public static readonly GEvent<int> UpdatePlayerHealth = new();
    
    public static readonly GEvent ShowBossHealth = new();
    public static readonly GEvent HideBossHealth = new();
    public static readonly GEvent<int> UpdateBossHealth = new();

    public static readonly GEvent<string> ShowOverlayText = new();
    public static readonly GEvent HideOverlayText = new();
    
    public static readonly GEvent OnBossDefeated = new();

    public static readonly GEvent OnCutsceneFinished = new();

}