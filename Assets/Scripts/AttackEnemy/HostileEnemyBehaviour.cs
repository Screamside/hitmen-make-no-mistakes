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
    [FoldoutGroup("Controller", nameof(stateOrder), nameof(animator), nameof(player), nameof(smgMan), nameof(batMan))]
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
    [SerializeField, HideField(nameof(batMan))]private Void _shootingGroup;
    [HideInInspector,HideField(nameof(batMan))]public float delayBeforeShooting;
    [HideInInspector, HideField(nameof(batMan))]public float delayAfterShooting;
    [HideInInspector, HideField(nameof(batMan))]public int shootingRange;
    [FormerlySerializedAs("pistolGameObject"), HideInInspector, HideField(nameof(batMan))]public GameObject weaponGameObject;
    [HideInInspector, HideField(nameof(batMan))]public Transform shootingPosition;
    [HideInInspector, HideField(nameof(batMan))]public GameObject bulletPrefab;
    
    [FoldoutGroup("Swing Bat State", nameof(delayBeforeAttack), nameof(prepareBatPosition), nameof(prepareBatTime), nameof(prepareBatAngle), nameof(delaySwingBat), nameof(swingBatTime), nameof(swingBatAngle), nameof(resetBatRotationTime), nameof(delayAfterSwing))]
    [SerializeField, HideField(nameof(batMan))]private Void _swingGroup;
    [HideInInspector]public float delayBeforeAttack;
    [Space]
    [HideInInspector]public Transform prepareBatPosition;
    [HideInInspector]public float prepareBatTime;
    [HideInInspector]public float prepareBatAngle;
    [Space]
    [HideInInspector]public float delaySwingBat;
    [HideInInspector]public float swingBatTime;
    [HideInInspector]public float swingBatAngle;
    [Space]
    [HideInInspector]public float resetBatRotationTime;
    [HideInInspector]public float delayAfterSwing;
    
    private readonly HostileEnemyStateFactory _stateFactory = new();
    private HostileEnemyState _currentState;

    public SpriteRenderer weaponSpriteRenderer;
    public GameObject warning;
    public float delayBetweenShots;

    private Vector3 _previousPosition;
    
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

        if (_previousPosition != transform.position)
        {
            animator.CrossFade("Walking", 0);
        }
        else
        {
            animator.CrossFade("Idle", 0);
        }
        
        UpdateWeaponDirection();
    }

    private void UpdateWeaponDirection()
    {
        if (batMan) { return;}

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
            yield return new WaitForSeconds(delayBetweenShots);
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