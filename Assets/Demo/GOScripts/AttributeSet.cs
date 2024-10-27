using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSet : MonoBehaviour
{
    public float health;
    public float currentHealth;
    public float maxHealth;
    public float currentMaxHealth;


    public float attack;
    public float defense;
    public float currentAttack;
    public float currentDefense;

    public delegate void LeathalHandler(GameObject instigator);
    public event LeathalHandler OnLeathal;

    public delegate void CurrentHealthHandler(float health);
    public event CurrentHealthHandler OnCurrentHealthChanged;

    public delegate void DeathHandler(GameObject instigator);
    public event DeathHandler OnDied;

    private void Start()
    {
        health = maxHealth;
        currentHealth = health;
        currentAttack = attack;
        currentDefense = defense;
    }

    internal void DealDamage(GameObject instigator, float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            OnLeathal.Invoke(instigator);
        }
        if (currentHealth <= 0) Die(instigator);
        else
        {
            SetCurrentHealth(currentHealth);
        }
    }

    private void SetCurrentHealth(float currentHealth)
    {
        OnCurrentHealthChanged.Invoke(currentHealth);
    }

    private void Die(GameObject instigator)
    {

        OnDied.Invoke(instigator);
    }
}
