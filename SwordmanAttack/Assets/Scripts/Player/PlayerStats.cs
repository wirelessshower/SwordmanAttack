using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance; 

    private float _baseDamage = 1;

    private float CurrentDamage => _baseDamage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void GainExperience(int damage)
    {
        _baseDamage += damage;
    }

    public float GetDamage()
    {
        return CurrentDamage;
    }

    
    
}

