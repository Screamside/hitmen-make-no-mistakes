using System;
using UnityEngine;

public class DestroyAfterFinish : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 1.5f);
    }
}
