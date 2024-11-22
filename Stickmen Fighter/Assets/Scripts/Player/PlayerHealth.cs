using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth = 3;

    private GameManager gameManager;
    private PlayerController playerController;
    public HealthBar healthBar;

    public void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerController = GetComponent<PlayerController>();
    }
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
        gameManager.OnPlayerDefeated(playerController);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(maxHealth);
    }
}
