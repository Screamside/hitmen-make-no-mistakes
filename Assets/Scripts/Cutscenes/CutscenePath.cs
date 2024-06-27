using UnityEngine;

public class CutscenePath : MonoBehaviour
{
    public Cutscene cutscene;
    
    public void ContinueCutscene()
    {
        StartCoroutine(cutscene.PlayCutscene());
    }
    
}
