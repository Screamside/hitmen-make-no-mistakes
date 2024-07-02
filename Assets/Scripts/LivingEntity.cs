using System;
using EditorAttributes;
using MelenitasDev.SoundsGood;
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
            new Sound(SFX.Die).SetOutput(Output.SFX).SetSpatialSound(false).Play();
            gameObject.SetActive(false);
            return;
        }
        
        new Sound(SFX.Damage).SetOutput(Output.SFX).SetSpatialSound(false).Play();
        
    }
    
}
