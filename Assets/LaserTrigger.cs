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
            sprite.sprite = disabledLasers;
            _animator.enabled = false;
            _collider2D.enabled = false;
            
            CutsceneManager.PlayMistake("Lasers");
        }
    }
    
    private IEnumerator CheckForMistakeCompletion()
    {
        while (true)
        {
            if (GameManager.IsMistakeDone("Lasers"))
            {
                sprite.sprite = disabledLasers;
                _animator.enabled = false;
                _collider2D.enabled = false;
            }
            else
            {
                sprite.sprite = enabledLasers;
                _animator.enabled = true;
                _collider2D.enabled = true;
            }
            
            yield return new WaitForSeconds(5f);
        }
    }
}
