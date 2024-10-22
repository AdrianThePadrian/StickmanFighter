using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;

    private void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. Current health: " + currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {

    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        GameManager.instance.OnPlayerDefeated(GetComponent<PlayerController>());
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
