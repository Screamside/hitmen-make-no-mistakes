using System.Collections;
using PrimeTween;
using UnityEngine;

public class HitmanKeysMistake : MonoBehaviour
{
    public GameObject hitman;
    public Transform firstPoint;
    public Transform secondPoint;
    [Range(1f, 10f)]public float speed = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GoAround());
    }

    private IEnumerator GoAround()
    {
        while (true)
        {
            yield return Tween.PositionAtSpeed(hitman.transform, secondPoint.position, speed).ToYieldInstruction();
            hitman.transform.localScale = new Vector3(-2, 2, 2);
            yield return Tween.PositionAtSpeed(hitman.transform, firstPoint.position, speed).ToYieldInstruction();
            hitman.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
