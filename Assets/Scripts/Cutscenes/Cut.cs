﻿using System;
using EditorAttributes;
using PrimeTween;
using Unity.Cinemachine;
using UnityEngine;
using Void = EditorAttributes.Void;

[Serializable]
public class Cut
{
    public CutType type;
    public int order;
    
    [EnableField(nameof(type), CutType.Movement)] public MovementCut movement;
    [EnableField(nameof(type), CutType.Dialogue)] public DialogueCut dialogue;
    [EnableField(nameof(type), CutType.Question)] public ChoiceCut choice;
}

public enum CutType
{
    Movement,
    Dialogue,
    Question
}

[Serializable]
public struct MovementCut
{
    public GameObject gameObject;
    public TweenSettings<Vector3> movementSettings;
    public bool isItCamera;
}

[Serializable]
public struct DialogueCut
{
    public float delay;
    [TextArea] public string dialogue;
}

[Serializable]
public struct ChoiceCut
{
    public string choiceLeft;
    public CutscenePath cutsceneLeft;
    public string choiceRight;
    public CutscenePath cutsceneRight;
}
