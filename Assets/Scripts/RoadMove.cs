using System;
using UnityEngine;

public class RoadMove : MonoBehaviour
{

    public float speed;
    public Material material;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
