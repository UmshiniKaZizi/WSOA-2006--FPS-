using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAttributes : MonoBehaviour
{
    [Header("Health Settings")]
    public float MaxHealth = 100f;
    public float CurrentHealth;

    [Header("Stamina Settings")]
    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float StaminaDrainRate = 40f; // Stamina drain per second when sprinting
    public float StaminaRecoveryRate = 5f; // Stamina recovery per second when idle
    public bool CanSprint => CurrentStamina > 0f;

    [Header("UI Elements")]
    public Slider healthSlider; // Assign in the inspector
    public Slider staminaSlider; // Assign in the inspector

    private bool isSprinting = false; // Tracks if the player is currently sprinting

    void Start()
    {
        // Initialize health
        CurrentHealth = MaxHealth;
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;

        // Initialize stamina
        CurrentStamina = MaxStamina;
        staminaSlider.maxValue = MaxStamina;
        staminaSlider.value = CurrentStamina;
    }

    void Update()
    {
        if (isSprinting)
        {
            DrainStamina();
        }
        else
        {
            RecoverStamina();
        }

        // Update stamina UI
        staminaSlider.value = CurrentStamina;
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
        Debug.Log("The player is dead. Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

    public void StartSprinting()
    {
        if (CanSprint)
        {
            isSprinting = true;
        }
    }

    public void StopSprinting()
    {
        isSprinting = false;
    }

    void DrainStamina()
    {
        if (CurrentStamina > 0f)
        {
            CurrentStamina -= StaminaDrainRate * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);
        }
        else
        {
            StopSprinting(); // Automatically stop sprinting when stamina depletes
        }
    }

    void RecoverStamina()
    {
        if (!isSprinting && CurrentStamina < MaxStamina)
        {
            CurrentStamina += StaminaRecoveryRate * Time.deltaTime;
            CurrentStamina = Mathf.Clamp(CurrentStamina, 0f, MaxStamina);
        }
    }
}
