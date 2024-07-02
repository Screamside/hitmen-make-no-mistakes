using System;
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
    
    [EnableField(nameof(type), CutType.SelectCamera)] public SelectCameraCut selectCamera;
    [EnableField(nameof(type), CutType.Movement)] public MovementCut movement;
    [EnableField(nameof(type), CutType.Animation)] public AnimationCut animation;
    [EnableField(nameof(type), CutType.Dialogue)] public DialogueCut dialogue;
    [EnableField(nameof(type), CutType.MistakeTitle)] public MistakeTitleCut mistakeTitle;
    [EnableField(nameof(type), CutType.MistakeTitle)] public PlayMistakeCut mistake;
    [EnableField(nameof(type), CutType.Delay)] public DelayCut delay;
    [EnableField(nameof(type), CutType.Question)] public ChoiceCut choice;
    [EnableField(nameof(type), CutType.Cutscene)] public PlayCutsceneCut cutscene;
}

public enum CutType
{
    SelectCamera,
    Movement,
    Animation,
    Dialogue,
    MistakeTitle,
    PlayMistake,
    Delay,
    WaitForInput,
    Question,
    Cutscene
}

[Serializable]
public struct SelectCameraCut
{
    public GameObject camera;
}

[Serializable]
public struct MovementCut
{
    public GameObject gameObject;
    public bool hasAnimation;
    public TweenSettings<Vector3> movementSettings;
}

[Serializable]
public struct AnimationCut
{
    public Animator animator;
    public string animationName;
}

[Serializable]
public struct PlayMistakeCut
{
    public string mistake;
}

[Serializable]
public struct PlayCutsceneCut
{
    public string cutsceneName;
}

[Serializable]
public struct DialogueCut
{
    public float delay;
    [TextArea] public string dialogue;
    public bool ignoreInput;
}

[Serializable]
public struct MistakeTitleCut
{
    public float delay;
    public string title;
}

[Serializable]
public struct DelayCut
{
    public float time;
}

[Serializable]
public struct ChoiceCut
{
    public string choiceLeft;
    public CutscenePath cutsceneLeft;
    public string choiceRight;
    public CutscenePath cutsceneRight;
}

