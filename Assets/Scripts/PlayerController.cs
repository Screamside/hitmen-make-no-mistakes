using System;
using MelenitasDev.SoundsGood;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D _rigidBody2d;
    private Animator _animator;
    private float _horizontalVelocity;
    private SpriteRenderer _sprite;
    private IInteractable _currentHoveredInteractable;
    private Vector2 _lastVelocity;
    public SceneInitializer currentScene;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GameObject interactPrompt;

    private Sound jumpSound;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        
        InputSystem.actions.FindAction("Move").started += context => _horizontalVelocity = context.ReadValue<float>();
        InputSystem.actions.FindAction("Move").performed += context => _horizontalVelocity = context.ReadValue<float>();
        InputSystem.actions.FindAction("Move").canceled += context => _horizontalVelocity = 0f;
        
        InputSystem.actions.FindAction("Jump").started += context =>
        {
            jumpSound.Play();
            _rigidBody2d.AddForceY(jumpForce);
        };
        
        InputSystem.actions.FindAction("Interact").started += Interact;
        
        jumpSound = new Sound(SFX.PlayerJump).SetSpatialSound(false);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            _currentHoveredInteractable = interactable;
            interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable _))
        {
            _currentHoveredInteractable = null;
            interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateAnimator();
    }

    private void FixedUpdate()
    {
        
        _rigidBody2d.velocity = (Vector2.right * _horizontalVelocity * speed) + _rigidBody2d.velocity.y * Vector2.up;
        
    }

    private void UpdateAnimator()
    {
        
        _animator.SetBool("isWalking", _horizontalVelocity != 0);

        if (_horizontalVelocity > 0)
        {
            _sprite.flipX = false;
        } else if (_horizontalVelocity < 0)
        {
            _sprite.flipX = true;
        }

        if (_rigidBody2d.velocityY > 0.001f)
        {
            _animator.SetBool("isJumping", true);
        }else
        {
            _animator.SetBool("isJumping", false);
        }
        
        if(_rigidBody2d.velocityY < -0.001f)
        {
            _animator.SetBool("isFalling", true);
        }
        else
        {
            _animator.SetBool("isFalling", false);
        }
        
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        
        if(_currentHoveredInteractable != null)
        {
            _currentHoveredInteractable.Interact();
        }
        
    }

    public void PausePhysics()
    {
        _rigidBody2d.simulated = false;
    }
    
    public void ResumePhysics()
    {
        _rigidBody2d.simulated = true;
    }
    
}
