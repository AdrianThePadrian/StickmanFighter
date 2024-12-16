using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    private float maxHealth = 3;

    public void Initialize(float health)
    {
        maxHealth = health;
        UpdateHealthBar(maxHealth);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }
}
