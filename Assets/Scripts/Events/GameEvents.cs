public static class GameEvents
{

    public static readonly GEvent<IInteractable> OnHoverInteractable = new();
    public static readonly GEvent<IInteractable> OnUnhoverInteractable = new();
    
    public static readonly GEvent OnChangeRoom = new();
    public static readonly GEvent<string> OnEnteredScene = new();

    public static readonly GEvent<int> OnPlayerMakesMistake = new();
    public static readonly GEvent<string> OnPlayerChooses = new();
    public static readonly GEvent OnPlayerKeyPressAfterDialogue = new();

    public static readonly GEvent OnCutsceneFinished = new();

}