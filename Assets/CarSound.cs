using System;
using MelenitasDev.SoundsGood;
using UnityEngine;

public class CarSound : MonoBehaviour
{

    private Sound car;
    [Range(-1, 1f)]public float basePitch;
    
    private Vector3 lastPosition;

    private void Awake()
    {
        car = new Sound(SFX.Car).SetSpatialSound(false).SetOutput(Output.SFX).SetLoop(true);
    }

    private void FixedUpdate()
    {
        float velocity = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        
        car.SetPitch(basePitch + velocity / 100f);
        car.SetVolume(velocity / 100f);
        car.Play();
    }
}
