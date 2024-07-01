using System.Collections;
using PrimeTween;
using UnityEngine;

public class HangingHitman : MonoBehaviour
{

    [Range(1f, 10f)]public float speed = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Swing());
    }

    private IEnumerator Swing()
    {

        while (true)
        {
            yield return Tween.Rotation(transform, Vector3.forward * -4, speed, Ease.InOutSine).ToYieldInstruction();
            yield return Tween.Rotation(transform, Vector3.forward * 4, speed, Ease.InOutSine).ToYieldInstruction();
        }
        
    }
}
