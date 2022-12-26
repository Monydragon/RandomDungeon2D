using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int currentHealth, maxHealth;

    private void Start()
    {
        EventManager.PlayerHealthChanged(currentHealth, maxHealth);
    }
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value >= maxHealth) { currentHealth = maxHealth; }
            else if (value <= 0) { currentHealth = 0; }
            else { currentHealth = value; }
            EventManager.PlayerHealthChanged(currentHealth, maxHealth);
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            EventManager.PlayerHealthChanged(currentHealth, maxHealth);
        }
    }
}