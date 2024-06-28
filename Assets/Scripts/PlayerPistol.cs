using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPistol : MonoBehaviour
{

    [SerializeField] private GameObject _pistol;
    [SerializeField] private SpriteRenderer _playerSprite;

    private Vector3 _mousePosition;

    [SerializeField, MinMaxSlider(-90f, 90f)]private Vector2 _angleClamp;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _cooldown;
    
    private float _cooldownTimer;
    private void Awake()
    {
        InputSystem.actions.FindAction("Shoot").started += (ctx) => Shoot();
        _cooldownTimer = _cooldown;
    }

    private void OnEnable()
    {
        _pistol.gameObject.SetActive(true);
        
    }

    private void OnDisable()
    {
        _pistol.gameObject.SetActive(false);
    }

    private void Shoot()
    {
        if (_cooldownTimer > 0) { return;}
        _cooldownTimer = _cooldown;
        
        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        
    }

    private void Update()
    {
        
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0;
        
        // Calculate the direction to the target
        Vector3 direction = _mousePosition - _pistol.transform.position;
        
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_mousePosition.x < transform.position.x)
        {
            _playerSprite.flipX = true;
            _pistol.transform.localScale = new Vector3(1, -1, 1);
            _pistol.transform.localPosition = new Vector3(-0.5f, 0.9f, 0);
            
            if (_mousePosition.y < _pistol.transform.position.y)
            {
                if (angle > _angleClamp.y - 180)
                {
                    angle = _angleClamp.y - 180;
                }
            }
            else
            {
                angle = Mathf.Clamp(angle, _angleClamp.x + 180, _angleClamp.y + 180);
            }
        }
        else
        {
            _playerSprite.flipX = false;
            _pistol.transform.localScale = new Vector3(1, 1, 1);
            _pistol.transform.localPosition = new Vector3(0.5f, 0.9f, 0); 
            
            angle = Mathf.Clamp(angle, _angleClamp.x, _angleClamp.y);
        }
        
        
        // Apply the rotation to the transform
        _pistol.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        
        if(_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
        
    }
}