using System;
using MelenitasDev.SoundsGood;
using PrimeTween;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHealth : MonoBehaviour
{

    public int health;
    public bool invincibility;
    private int currentHealth;
    public SpriteRenderer sprite;

    private void Awake()
    {
        currentHealth = health;
    }

    public void Damage(int amount)
    {
        //TODO update UI
        if(invincibility) return;

        currentHealth -= amount;
        
        GameEvents.UpdatePlayerHealth.Invoke(currentHealth);

        Tween.Color(sprite, Color.red, 0.2f).OnComplete(() => sprite.color = Color.white);

        if (currentHealth <= 0)
        {
            currentHealth = health;
            GameManager.RestartFromMistake("DiedFromBullet");
            
            new Sound(SFX.Die).SetSpatialSound(false).SetOutput(Output.SFX).Play();
            
            return;
        }
        
        new Sound(SFX.Damage).SetSpatialSound(false).SetOutput(Output.SFX).Play();

    }
    
}
