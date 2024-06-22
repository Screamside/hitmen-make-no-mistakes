using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D _rigidBody2d;
    private Animator _animator;
    private float _horizontalVelocity;
    private SpriteRenderer _sprite;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 10f;

    private void Awake()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        
        InputSystem.actions.FindAction("Move").started += context => _horizontalVelocity = context.ReadValue<float>();
        InputSystem.actions.FindAction("Move").performed += context => _horizontalVelocity = context.ReadValue<float>();
        InputSystem.actions.FindAction("Move").canceled += context => _horizontalVelocity = 0f;
        
        InputSystem.actions.FindAction("Jump").started += context => _rigidBody2d.AddForceY(jumpForce);
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
}
