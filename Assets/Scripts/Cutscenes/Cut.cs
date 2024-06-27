using System;
using NaughtyAttributes;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public class Cut
{
    [OnValueChanged("UpdateEditor")]
    public CutType type;

    [BoxGroup("Movement")]
    public CinemachineCamera cinemachineCamera;
    public TweenSettings<Vector3> movementSettings;
    public bool waitForCameraAnimation;

    [BoxGroup("Dialogue")] 
    public float dialogueDelay;
    [BoxGroup("Dialogue")]
    [TextArea] public string dialogue;
    
    [BoxGroup("Question")]
    public string choiceLeft;
    //public Cutscene cutsceneLeft;
    
    [BoxGroup("Question")]
    public string choiceRight;
    //public Cutscene cutsceneRight;
    
}

public enum CutType
{
    Movement,
    Dialogue,
    Question
}
