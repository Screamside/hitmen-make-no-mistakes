using System;
using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Void = EditorAttributes.Void;

public class HostileEnemyBehaviour : MonoBehaviour
{
    [FoldoutGroup("Controller", nameof(stateOrder), nameof(animator), nameof(player))]
    [SerializeField]private Void _controllerGroup;
    [HideInInspector]public List<HostileEnemyStateType> stateOrder;
    [HideInInspector]public Animator animator;
    [HideInInspector]public PlayerController player;
    [HideInInspector]public bool smgMan = false;
    [HideInInspector]public bool batMan = false;
    
    [FoldoutGroup("Moving State", nameof(moveSpeed), nameof(minDistanceToShoot), nameof(maxDistanceToRunAway), nameof(layermask))]
    [SerializeField]private Void _movingGroup;
    [HideInInspector]public float moveSpeed;
    [HideInInspector]public float minDistanceToShoot;
    [HideInInspector]public float maxDistanceToRunAway;
    [HideInInspector]public LayerMask layermask;
    
    [FoldoutGroup("Shooting State", nameof(delayBeforeShooting), nameof(delayAfterShooting), nameof(shootingPosition), nameof(weaponGameObject), nameof(bulletPrefab), nameof(shootingRange))]
    [SerializeField]private Void _shootingGroup;
    [HideInInspector]public float delayBeforeShooting;
    [HideInInspector]public float delayAfterShooting;
    [HideInInspector]public float shootingRange;
    [FormerlySerializedAs("pistolGameObject")] [HideInInspector]public GameObject weaponGameObject;
    [HideInInspector]public Transform shootingPosition;
    [HideInInspector]public GameObject bulletPrefab;

    [HideInInspector] public bool canRotateWeapon;
    
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

        if(transform.position.x >player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        UpdateWeaponDirection();
    }

    private void UpdateWeaponDirection()
    {
        if (!canRotateWeapon) { return;}

        weaponGameObject.transform.LookAt(player.transform.position + player.transform.up);

        if(transform.position.x >player.transform.position.x)
        {
            weaponGameObject.transform.right = -weaponGameObject.transform.forward;
        }
        else
        {
            weaponGameObject.transform.right = weaponGameObject.transform.forward;
        }
    }

    public void SpawnBullet()
    {

        if (smgMan)
        {
            StartCoroutine(SpawnSMGBullets());
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = shootingPosition.position;
        bullet.transform.LookAt(player.transform.position + Vector3.up);
        bullet.transform.right = bullet.transform.forward;
        Vector3 originalRotation = bullet.transform.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingRange, -shootingRange));
        
    }

    private IEnumerator SpawnSMGBullets()
    {

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = shootingPosition.position;
            bullet.transform.LookAt(player.transform.position + Vector3.up);
            bullet.transform.right = bullet.transform.forward;
            Vector3 originalRotation = bullet.transform.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingRange, -shootingRange));
            yield return new WaitForSeconds(0.2f);
        }
        
    }
}

public class HostileEnemyStateFactory
{
 
    private ShootingState _shooting = new ();
    private MovingCloserState _movingCloser = new ();
    private MovingAwayState _movingAway = new ();
    private SwingBatState _swing = new ();
    
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
            case HostileEnemyStateType.Swing:
                return _swing;
        }
        
        return null;
    }

}

public enum HostileEnemyStateType
{
    Shooting,
    MovingAway,
    MovingCloser,
    Swing
    
}