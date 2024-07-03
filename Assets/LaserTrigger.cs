using System;
using System.Collections;
using UnityEngine;

public class LaserTrigger : MonoBehaviour
{

    public Sprite disabledLasers;
    public Sprite enabledLasers;
    public SpriteRenderer sprite;
    private Animator _animator;
    private BoxCollider2D _collider2D;

    private bool triggered = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();

        StartCoroutine(CheckForMistakeCompletion());
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            triggered = true;
            _collider2D.enabled = false;
            sprite.sprite = disabledLasers;
            _animator.enabled = false;
            
            CutsceneManager.PlayMistake("Lasers");
        }
    }
    
    private IEnumerator CheckForMistakeCompletion()
    {
        while (true)
        {
            if (GameManager.IsMistakeDone("Lasers") || triggered)
            {
                _animator.enabled = false;
                sprite.sprite = disabledLasers;
                _collider2D.enabled = false;
            }
            else
            {
                _animator.enabled = true;
                sprite.sprite = enabledLasers;
                _collider2D.enabled = true;
            }
            
            yield return new WaitForSeconds(5f);
        }
    }
}
