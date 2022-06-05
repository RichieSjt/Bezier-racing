using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public HealthBar healthBar;
    public float maxHealth;
    private float currentHealth;
    public static event Action playerDie;

    // Subscribing the method "setDamage" to the event Action "playerHit" when the class enables
    private void OnEnable()
    {
        ParticleSystem.playerHit += setDamage;
    }

    // Unsubscribing the method "setDamage" to the event Action "playerHit" when the class disables
    private void OnDisable()
    {
        ParticleSystem.playerHit -= setDamage;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void setDamage(float damage)
    {
        if(currentHealth <= 0)
        {
            playerDie?.Invoke();
            healthBar.setSize(0);
        }
        
        // Reduce the current health and set the bar size
        currentHealth -= damage;
        healthBar.setSize(normalizedHealth());
    }

    // Returns the current health in a value beetween 0 - 1
    private float normalizedHealth()
    {
        return currentHealth / maxHealth;
    }
}