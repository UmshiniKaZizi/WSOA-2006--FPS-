using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttributes : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float CurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
    }
    void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("The players is dead");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
