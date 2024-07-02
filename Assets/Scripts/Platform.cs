using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;

public class Platform : MonoBehaviour
{
    
    private bool isPlayerTouching = false;
    private BoxCollider2D _collider2D;

    public LayerMask excludePlayer;
    public LayerMask normalLayer;

    private void Awake()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        InputSystem.actions.FindAction("Down").started += OnDown;
    }

    private void OnDisable()
    {
        InputSystem.actions.FindAction("Down").started -= OnDown;
    }
    
    private void OnDown(InputAction.CallbackContext obj)
    {
        _collider2D.excludeLayers = excludePlayer;
        
        Tween.Delay(1f, () =>
        {
            _collider2D.excludeLayers = normalLayer;
            isPlayerTouching = false;
        });
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            isPlayerTouching = true;
        }
    }

}
