using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using Random = UnityEngine.Random;
using Void = EditorAttributes.Void;

public class BossBehaviour : MonoBehaviour
{
    [FoldoutGroup("Controller", nameof(stateOrder), nameof(animator), nameof(player))]
    [SerializeField]private Void _controllerGroup;
    [HideInInspector]public List<BossStateType> stateOrder;
    [HideInInspector]public Animator animator;
    [HideInInspector]public PlayerController player;
    
    [FoldoutGroup("Moving State", nameof(moveSpeed), nameof(minDistanceToShoot), nameof(maxDistanceToRunAway), nameof(layermask))]
    [SerializeField]private Void _movingGroup;
    [HideInInspector]public float moveSpeed;
    [HideInInspector]public float minDistanceToShoot;
    [HideInInspector]public float maxDistanceToRunAway;
    [HideInInspector]public LayerMask layermask;
    
    [FoldoutGroup("Shooting State", nameof(delayBeforeShooting), nameof(delayAfterShooting), nameof(bulletPrefab), nameof(bulletSpeed))]
    [SerializeField]private Void _shootingGroup;
    [HideInInspector]public float delayBeforeShooting;
    [HideInInspector]public float delayAfterShooting;
    [HideInInspector]public int shootingAngle;
    [Space]
    [HideInInspector]public GameObject bulletPrefab;
    [HideInInspector]public float bulletSpeed;
    
    [FoldoutGroup("SMG", nameof(smgObject), nameof(smgShootingPosition))]
    [SerializeField]private Void _smg;
    [HideInInspector]public GameObject smgObject;
    [HideInInspector]public Transform smgShootingPosition;
    
    [FoldoutGroup("Gun", nameof(smgObject), nameof(gunShootingPosition))]
    [SerializeField]private Void _gun;
    [HideInInspector]public GameObject gunObject;
    [HideInInspector]public Transform gunShootingPosition;
    
    [FoldoutGroup("Bat", nameof(batObject))]
    [SerializeField]private Void _bat;
    [HideInInspector]public GameObject batObject;
    
    [FoldoutGroup("Swing Bat State", nameof(delayBeforeAttack), nameof(prepareBatPosition), nameof(prepareBatTime), nameof(prepareBatAngle), nameof(delaySwingBat), nameof(swingBatTime), nameof(swingBatAngle), nameof(resetBatRotationTime), nameof(delayAfterSwing))]
    [SerializeField]private Void _swingGroup;
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
    
    private readonly BossStateFactory _stateFactory = new();
    private BossState _currentState;

    public SpriteRenderer weaponSpriteRenderer;
    public GameObject warning;
    public float delayBetweenShots;
    private int stateIndex = 0;

    private Vector3 _previousPosition;
    
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        
        _currentState = _stateFactory.GetStateFromType(stateOrder[stateIndex]);
        stateIndex++;
        _currentState.EnterState(this);
    }

    public void SwitchToNextState()
    {
        _currentState.ExitState();
        
        _currentState = _stateFactory.GetStateFromType(stateOrder[stateIndex]);
        stateIndex++;
        
        
        
        _currentState.EnterState(this);

    }

    private void Update()
    {
        _currentState.UpdateState();
        UpdateWeaponDirection();
        
        if (_previousPosition != transform.position)
        {
            animator.CrossFade("Walking", 0);
        }
        else
        {
            animator.CrossFade("Idle", 0);
        }

        if (_currentState.GetType() == typeof(BossSwingBatState))
        {
            return;
        }
        
        if(transform.position.x >player.transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        _previousPosition = transform.position;
        
    }

    private void UpdateWeaponDirection()
    {
        smgObject.transform.LookAt(player.transform.position + player.transform.up);
        gunObject.transform.LookAt(player.transform.position + player.transform.up);

        if(transform.position.x >player.transform.position.x)
        {
            smgObject.transform.right = -smgObject.transform.forward;
            gunObject.transform.right = -gunObject.transform.forward;
        }
        else
        {
            smgObject.transform.right = smgObject.transform.forward;
            gunObject.transform.right = gunObject.transform.forward;
        }
    }

    public void SpawnGunBullet()
    {

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gunShootingPosition.position;
        bullet.transform.LookAt(player.transform.position + Vector3.up);
        bullet.transform.right = bullet.transform.forward;
        Vector3 originalRotation = bullet.transform.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingAngle, -shootingAngle));
        
    }
    
    public void SpawnSMGBullet()
    {
        StartCoroutine(SMGBullets());
    }

    private IEnumerator SMGBullets()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = smgShootingPosition.position;
            bullet.transform.LookAt(player.transform.position + Vector3.up);
            bullet.transform.right = bullet.transform.forward;
            Vector3 originalRotation = bullet.transform.eulerAngles;
            bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingAngle, -shootingAngle));
            yield return new WaitForSeconds(delayBetweenShots);
        }
        
    }
}

public class BossStateFactory
{
 
    private BossShootingGunState _shootingGun = new ();
    private BossShootingSMGState _shootingSMG = new ();
    private BossMovingCloserState _movingCloser = new ();
    private BossMovingAwayState _movingAway = new ();
    private BossSwingBatState _swing = new ();
    
    public BossState GetStateFromType(BossStateType stateType)
    {

        switch (stateType)
        {
            case BossStateType.MovingAway:
                return _movingAway;
            case BossStateType.MovingCloser:
                return _movingCloser;
            case BossStateType.ShootingGun:
                return _shootingGun;
            case BossStateType.ShootingSMG:
                return _shootingSMG;
            case BossStateType.Swing:
                return _swing;
        }
        
        return null;
    }

}

public enum BossStateType
{
    ShootingGun,
    ShootingSMG,
    Dynamite,
    MovingAway,
    MovingCloser,
    Swing,
    ChangeToGun,
    ChangeToSMG,
    ChangeToSwing
}