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
    public bool stopControlling;

    [SerializeField] private float speed = 10f;
    [SerializeField] public float ladderSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private PlayerPistol _playerPistol;
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private LayerMask _groundLayer;

    private Sound jumpSound;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _playerPistol = GetComponent<PlayerPistol>();
        
        EnableInput();
        
        jumpSound = new Sound(SFX.PlayerJump).SetSpatialSound(false).SetOutput(Output.SFX);
        
    }

    public void EnablePistol()
    {
        _playerPistol.enabled = true;
    }
    
    public void DisablePistol()
    {
        _playerPistol.enabled = false;
    }

    private void UpdateHorizontalVelocity(InputAction.CallbackContext context) => _horizontalVelocity = context.ReadValue<float>();
    private void ResetHorizontalVelocity(InputAction.CallbackContext context) => _horizontalVelocity = 0;

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(_groundTransform.position, 0.1f, _groundLayer);
    }
    
    private void UpdateJump(InputAction.CallbackContext context) {
        if (IsGrounded())
        {
            jumpSound.Play();
            _rigidBody2d.linearVelocityY = jumpForce;
        }
    }

    public void EnableInput()
    {
        InputSystem.actions.FindAction("Move").started += UpdateHorizontalVelocity;
        InputSystem.actions.FindAction("Move").performed += UpdateHorizontalVelocity;
        InputSystem.actions.FindAction("Move").canceled += ResetHorizontalVelocity;

        InputSystem.actions.FindAction("Jump").started += UpdateJump;
        
        InputSystem.actions.FindAction("Interact").started += Interact;
    }

    public void DisableInput()
    {
        InputSystem.actions.FindAction("Move").started -= UpdateHorizontalVelocity;
        InputSystem.actions.FindAction("Move").performed -= UpdateHorizontalVelocity;
        InputSystem.actions.FindAction("Move").canceled -= ResetHorizontalVelocity;
        ResetHorizontalVelocity(default);

        InputSystem.actions.FindAction("Jump").started -= UpdateJump;
        
        InputSystem.actions.FindAction("Interact").started -= Interact;
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
        if (stopControlling) return;
        
        _rigidBody2d.linearVelocity = (Vector2.right * _horizontalVelocity * speed) + _rigidBody2d.linearVelocity.y * Vector2.up;
    }

    private void UpdateAnimator()
    {
        _animator.SetBool("isWalking", _horizontalVelocity != 0);

        if (!_playerPistol.enabled)
        {
            if (_horizontalVelocity < 0)
            {
                _sprite.flipX = true;
            }
            else if (_horizontalVelocity > 0)
            {
                _sprite.flipX = false;
            }
        }

        if (_rigidBody2d.linearVelocityY > 0.001f)
        {
            _animator.SetBool("isJumping", true);
        }
        else
        {
            _animator.SetBool("isJumping", false);
        }
        
        if(_rigidBody2d.linearVelocityY < -0.001f)
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
