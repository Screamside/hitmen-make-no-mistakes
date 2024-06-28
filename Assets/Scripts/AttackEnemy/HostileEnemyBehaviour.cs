using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using Void = EditorAttributes.Void;

public class HostileEnemyBehaviour : MonoBehaviour
{
    [FoldoutGroup("Controller", nameof(stateOrder), nameof(animator), nameof(player))]
    [SerializeField]private Void _controllerGroup;
    [HideInInspector]public List<HostileEnemyStateType> stateOrder;
    [HideInInspector]public Animator animator;
    [HideInInspector] public PlayerController player;
    
    [FoldoutGroup("Moving State", nameof(moveSpeed), nameof(minDistanceToShoot), nameof(maxDistanceToRunAway), nameof(layermask))]
    [SerializeField]private Void _movingGroup;
    [HideInInspector]public float moveSpeed;
    [HideInInspector]public float minDistanceToShoot;
    [HideInInspector]public float maxDistanceToRunAway;
    [HideInInspector]public LayerMask layermask;
    
    [FoldoutGroup("Shooting State", nameof(delayBeforeShooting), nameof(delayAfterShooting))]
    [SerializeField]private Void _shootingGroup;
    [HideInInspector]public float delayBeforeShooting;
    [HideInInspector]public float delayAfterShooting;

    private readonly HostileEnemyStateFactory _stateFactory = new();
    private HostileEnemyState _currentState;

    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        
        _currentState = _stateFactory.GetStateFromType(HostileEnemyStateType.MovingCloser);
        _currentState.EnterState(this);
    }

    public void SwitchState(HostileEnemyStateType stateType)
    {
        _currentState.ExitState();
        
        _currentState = _stateFactory.GetStateFromType(stateType);
        
        _currentState.EnterState(this);

    }

    private void Update()
    {
        _currentState.UpdateState();

        if(player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}

public class HostileEnemyStateFactory
{
 
    private ShootingState _shooting = new ();
    private MovingCloserState _movingCloser = new ();
    private MovingAwayState _movingAway = new ();
    
    public HostileEnemyState GetStateFromType(HostileEnemyStateType stateType)
    {

        switch (stateType)
        {
            case HostileEnemyStateType.MovingAway:
                return _movingAway;
            case HostileEnemyStateType.MovingCloser:
                return _movingCloser;
            case HostileEnemyStateType.Shooting:
                return _shooting;
        }
        
        return null;
    }

}

public enum HostileEnemyStateType
{
    Shooting,
    MovingAway,
    MovingCloser
    
}