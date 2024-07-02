using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ladder : MonoBehaviour
{

    private PlayerController player;
    private bool isPlayerOnLadder;
    private float horizontalMovement;
    private float verticalMovement;
    public BoxCollider2D boxCollider;

    private void Awake()
    {
        InputSystem.actions.FindAction("Move").canceled += ctx => horizontalMovement = 0f;
        InputSystem.actions.FindAction("MoveVertical").canceled += ctx => verticalMovement = 0f;
    }

    private void OnEnable()
    {
        InputSystem.actions.FindAction("OnPressUpOrDown").started += OnUpDown;
        InputSystem.actions.FindAction("Move").started += UpdateHorizontal;
        InputSystem.actions.FindAction("Move").performed += UpdateHorizontal;
        
        InputSystem.actions.FindAction("MoveVertical").started += UpdateVertical;
        InputSystem.actions.FindAction("MoveVertical").performed += UpdateVertical;
        
    }

    private void OnDisable()
    {
        InputSystem.actions.FindAction("OnPressUpOrDown").started -= OnUpDown;
        InputSystem.actions.FindAction("Move").started -= UpdateHorizontal;
        InputSystem.actions.FindAction("Move").performed -= UpdateHorizontal;
        InputSystem.actions.FindAction("MoveVertical").started -= UpdateVertical;
        InputSystem.actions.FindAction("MoveVertical").performed -= UpdateVertical;
    }

    private void UpdateVertical(InputAction.CallbackContext obj)
    {
        verticalMovement = obj.ReadValue<float>();
        if (player == null) { return; }
        player._rigidBody2d.gravityScale = 0;
        boxCollider.enabled = false;
        player.stopControlling = true;
        isPlayerOnLadder = true;
    }

    private void UpdateHorizontal(InputAction.CallbackContext obj)
    {
        horizontalMovement = obj.ReadValue<float>();
    }
    
    private void OnUpDown(InputAction.CallbackContext obj)
    {
        if (player == null) { return; }
        player._rigidBody2d.gravityScale = 0;
        boxCollider.enabled = false;
        player.stopControlling = true;
        isPlayerOnLadder = true;
    }

    private void FixedUpdate()
    {
        if (player == null || !isPlayerOnLadder)
        {
            return;
        }
        
        player._rigidBody2d.velocity = new Vector2(horizontalMovement, verticalMovement).normalized * player.ladderSpeed;
        Debug.Log(player._rigidBody2d.velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnLadder = false;
            boxCollider.enabled = true;
            player._rigidBody2d.gravityScale = 4;
            player.stopControlling = false;
            player = null;
        }
    }
}
