using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    public HealthBar healthBar;

    private void OnPlayerJoined()
    {
        healthBar.Initialize(maxHealth);
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth);
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        GameManager.instance.OnPlayerDefeated(GetComponent<PlayerController>());
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth);
    }
}
