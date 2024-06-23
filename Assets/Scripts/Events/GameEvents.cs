public static class GameEvents
{

    public static readonly GEvent<IInteractable> OnHoverInteractable = new();
    public static readonly GEvent<IInteractable> OnUnhoverInteractable = new();
    
    public static readonly GEvent OnChangeRoom = new();
    public static readonly GEvent<string> OnEnteredScene = new();

}