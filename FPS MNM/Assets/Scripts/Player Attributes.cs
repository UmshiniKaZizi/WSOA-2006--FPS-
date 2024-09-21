using UnityEngine;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float CurrentHealth;

    [Header("UI Elements")]
    public Slider healthSlider; // Assign in the inspector

    void Start()
    {
        CurrentHealth = MaxHealth;
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        healthSlider.value = CurrentHealth; // Update the health slider

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("The player is dead");
        // Implement death logic (e.g., reload scene, show game over screen, etc.)
    }
}
