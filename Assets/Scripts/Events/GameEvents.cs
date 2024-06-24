public static class GameEvents
{

    public static readonly GEvent<IInteractable> OnHoverInteractable = new();
    public static readonly GEvent<IInteractable> OnUnhoverInteractable = new();
    
    public static readonly GEvent OnChangeRoom = new();
    public static readonly GEvent<string> OnEnteredScene = new();

    public static readonly GEvent<int> OnPlayerMakesMistake = new();

    public static readonly GEvent AskForFadeIn = new();
    public static readonly GEvent AskForFadeOut = new();
    public static readonly GEvent<string> AskForDialogueAnimation = new();

}