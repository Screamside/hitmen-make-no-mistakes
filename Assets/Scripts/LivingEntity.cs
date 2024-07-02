using System;
using EditorAttributes;
using PrimeTween;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public int health;
    [ReadOnly]public int currentHealth;
    public SpriteRenderer sprite;

    private void OnEnable()
    {
        currentHealth = health;
    }

    public void Damage(int amount)
    {
        Tween.Color(sprite, Color.white, Color.red, 0.05f).OnComplete(() => sprite.color = Color.white);
        
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject, 10f);
            gameObject.SetActive(false);
        }
        
    }
}
