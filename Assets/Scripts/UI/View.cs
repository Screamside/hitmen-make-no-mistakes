using UnityEngine;

public abstract class View : MonoBehaviour
{

    public abstract void InitializeView(UIController uiController);
    
    public abstract void UpdateView();

    public abstract void ExitView();

}
