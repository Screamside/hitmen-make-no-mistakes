using EditorAttributes;
using MelenitasDev.SoundsGood;
using PrimeTween;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health;
    [ReadOnly]public int currentHealth;
    public SpriteRenderer sprite;

    private Sound damage;
    private Sound die;

    private void Awake()
    {
        damage = new Sound(SFX.Damage).SetOutput(Output.SFX).SetSpatialSound(false);
        die = new Sound(SFX.Die).SetOutput(Output.SFX).SetSpatialSound(false);
    }

    private void OnEnable()
    {
        currentHealth = health;
        GameEvents.UpdateBossHealth.Invoke(currentHealth);
    }

    public void Damage(int amount)
    {
        Tween.Color(sprite, Color.white, Color.red, 0.05f).OnComplete(() => sprite.color = Color.white);
        
        currentHealth -= amount;
        
        GameEvents.UpdateBossHealth.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Tween.StopAll(gameObject);
            die.Play();
            gameObject.SetActive(false);
            GameEvents.OnBossDefeated.Invoke();
            return;
        }
        
        damage.Play();
        
    }
}
