using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    
    public Cutscene cutscene;
    
    public void PlayCutscene()
    {
        StartCoroutine(cutscene.PlayCutscene());
    }
    
}
