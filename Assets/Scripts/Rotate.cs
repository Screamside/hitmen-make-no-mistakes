using PrimeTween;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    public ShakeSettings ShakeSettings;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tween.ShakeLocalPosition(transform, ShakeSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
