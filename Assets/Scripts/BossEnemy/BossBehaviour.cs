using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using Unity.Cinemachine;
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
    
    [FoldoutGroup("Moving State", nameof(moveSpeed), nameof(layermask))]
    [SerializeField]private Void _movingGroup;
    [HideInInspector]public float moveSpeed;
    [HideInInspector]public LayerMask layermask;
    
    [FoldoutGroup("Shooting State", nameof(delayBeforeShooting), nameof(delayAfterShooting), nameof(shootingAngle), nameof(bulletPrefab), nameof(bulletSpeed))]
    [SerializeField]private Void _shootingGroup;
    [HideInInspector]public float delayBeforeShooting;
    [HideInInspector]public float delayAfterShooting;
    [HideInInspector]public int shootingAngle;
    [Space]
    [HideInInspector]public GameObject bulletPrefab;
    [HideInInspector]public float bulletSpeed;
    
    [FoldoutGroup("SMG", nameof(smgObject), nameof(smgShootingPosition), nameof(minDistanceToShootSMG), nameof(maxDistanceToRunAwaySMG))]
    [SerializeField]private Void _smg;
    [HideInInspector]public GameObject smgObject;
    [HideInInspector]public Transform smgShootingPosition;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 minDistanceToShootSMG;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 maxDistanceToRunAwaySMG;
    
    [FoldoutGroup("Gun", nameof(gunObject), nameof(gunShootingPosition), nameof(minDistanceToShootGun), nameof(maxDistanceToRunAwayGun))]
    [SerializeField]private Void _gun;
    [HideInInspector]public GameObject gunObject;
    [HideInInspector]public Transform gunShootingPosition;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 minDistanceToShootGun;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 maxDistanceToRunAwayGun;
    
    [FoldoutGroup("Bat", nameof(batObject), nameof(minDistanceToSwing), nameof(maxDistanceToRunAwayBat))]
    [SerializeField]private Void _bat;
    [HideInInspector]public GameObject batObject;
    [HideInInspector]public float minDistanceToSwing;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 maxDistanceToRunAwayBat;
    
    [FoldoutGroup("Dynamite", nameof(dynamiteObject), nameof(dynamitePrefab), nameof(delayBeforeThrow), nameof(delayAfterThrow), nameof(dynamiteSpawnPoint), nameof(throwPower))]
    [SerializeField]private Void _dynamite;
    [HideInInspector]public GameObject dynamiteObject;
    [HideInInspector]public GameObject dynamitePrefab;
    [HideInInspector]public float delayBeforeThrow;
    [HideInInspector]public float delayAfterThrow;
    [HideInInspector, MinMaxRangeSlider(0f, 20f)]public Vector2 throwPower;
    [HideInInspector]public Transform dynamiteSpawnPoint;
    
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
    public int stateIndex = 0;

    private Vector3 _previousPosition;

    public bool skipMoveAway;
    
    private void Awake()
    {
        player = FindFirstObjectByType<PlayerController>();
        
        dynamiteObject.SetActive(false);
        batObject.SetActive(false);
        smgObject.SetActive(false);
        gunObject.SetActive(true);
        stateIndex++;
        _currentState = _stateFactory.GetStateFromType(stateOrder[stateIndex]);
        stateIndex++;
        _currentState.EnterState(this);
    }

    public void SwitchToNextState()
    {
        
        if(stateIndex >= stateOrder.Count)
        {
            stateIndex = 0;
        }
        
        _currentState.ExitState();
        
        switch (stateOrder[stateIndex])
        {
            case BossStateType.ChangeToGun:
                dynamiteObject.SetActive(false);
                batObject.SetActive(false);
                smgObject.SetActive(false);
                gunObject.SetActive(true);
                stateIndex++;
                break;
            
            case BossStateType.ChangeToSMG:
                dynamiteObject.SetActive(false);
                batObject.SetActive(false);
                gunObject.SetActive(false);
                smgObject.SetActive(true);
                stateIndex++;
                break;
            
            case BossStateType.ChangeToSwing:
                dynamiteObject.SetActive(false);
                batObject.SetActive(true);
                gunObject.SetActive(false);
                smgObject.SetActive(false);
                stateIndex++;
                break;
            
            case BossStateType.ChangeToDynamite:
                dynamiteObject.SetActive(true);
                batObject.SetActive(false);
                gunObject.SetActive(false);
                smgObject.SetActive(false);
                stateIndex++;
                break;
        }
        
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
    
    public void SpawnDynamite()
    {
        GameObject dynamite = Instantiate(dynamitePrefab);
        dynamite.transform.position = dynamiteSpawnPoint.position;
        Dynamite d = dynamite.GetComponent<Dynamite>();
        
        if(transform.lossyScale.x < 0)
        {
            d.direction = -dynamiteSpawnPoint.right;
        }
        else
        {
            d.direction = dynamiteSpawnPoint.right;
        }
        
        
        d.throwForce = Random.Range(throwPower.x, throwPower.y);

    }

    public void SpawnGunBullet()
    {

        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = gunShootingPosition.position;
        bullet.transform.LookAt(player.transform.position + Vector3.up);
        bullet.transform.right = bullet.transform.forward;
        bullet.GetComponent<Bullet>().owner = "enemy";
        Vector3 originalRotation = bullet.transform.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingAngle, -shootingAngle));
        
    }
    
    public void SpawnSMGBullet()
    {
        StartCoroutine(SMGBullets());
    }
    

    private IEnumerator SMGBullets()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = smgShootingPosition.position;
            bullet.transform.LookAt(player.transform.position + Vector3.up);
            bullet.transform.right = bullet.transform.forward;
            Vector3 originalRotation = bullet.transform.eulerAngles;
            bullet.GetComponent<Bullet>().owner = "enemy";
            bullet.transform.rotation = Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z + Random.Range(shootingAngle, -shootingAngle));
            yield return new WaitForSeconds(delayBetweenShots);
        }
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Wall"))
        {
            skipMoveAway = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.CompareTag("Wall"))
        {
            skipMoveAway = false;
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
    private BossDynamiteState _dynamite = new ();
    
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
            case BossStateType.Dynamite:
                return _dynamite;
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
    ChangeToSwing,
    ChangeToDynamite
}