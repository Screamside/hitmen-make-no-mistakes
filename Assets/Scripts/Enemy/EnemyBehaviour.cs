using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

public class EnemyBehaviour : MonoBehaviour
{

    private EnemyState _currentState;

    [FoldoutGroup("Controller", nameof(stateOrder), nameof(animator))]
    [SerializeField]private Void _controllerGroup;
    [HideInInspector]public List<EnemyStateType> stateOrder;
    [HideInInspector]public Animator animator;
    
    [FoldoutGroup("Alert State", nameof(alertTimer), nameof(randomizeTimer), nameof(randomizeRange))]
    [SerializeField]private Void _alertGroup;
    [HideInInspector][DisableField(nameof(randomizeTimer))]public float alertTimer;
    [HideInInspector]public bool randomizeTimer;
    [HideInInspector][MinMaxSlider(0f, 20f)] public Vector2 randomizeRange;
    
    [FoldoutGroup("Moving State", nameof(points), nameof(movementSpeed), nameof(isStatic))]
    [SerializeField]private Void _movingGroup;
    [HideInInspector, DisableField(nameof(isStatic))]public List<Transform> points;
    [HideInInspector, DisableField(nameof(isStatic))]public float movementSpeed;
    [HideInInspector]public bool isStatic;

    private int _currentIndex; 
    private readonly EnemyStateFactory _stateFactory = new();

    private Vector3 _lastPosition;

    public bool canWalk = true;

    private void Awake()
    {
        _currentState = _stateFactory.GetStateFromType(stateOrder[_currentIndex]);
        _currentState.EnterState(this);
    }

    public void SwitchToNextStateInList()
    {
        _currentState.ExitState();
        
        _currentState = _stateFactory.GetStateFromType(stateOrder[_currentIndex]);
        
        _currentState.EnterState(this);

        _currentIndex++;
        
        if(_currentIndex == stateOrder.Count)
        {
            _currentIndex = 0;
        }
    }

    private void Update()
    {
        _currentState.UpdateState();

        if (_lastPosition != transform.position)
        {
            if(_lastPosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (_lastPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        
        _lastPosition = transform.position;
    }
}

public class EnemyStateFactory
{

    private AlertState _alertState = new();
    private MovingState _movingState = new(); 
    
    public EnemyState GetStateFromType(EnemyStateType stateType)
    {
        switch (stateType)
        {
            case EnemyStateType.Alert:
                return _alertState;
            case EnemyStateType.Move:
                return _movingState;
            default:
                return null;
        }
    }

}

public enum EnemyStateType
{
    Alert,
    Move
    
}