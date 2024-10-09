using UnityEngine;

public class PlayerAmmoPickup : MonoBehaviour
{
    public Weapon weapon; // Reference to the Weapon script

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "AmmoPack" tag
        if (other.CompareTag("AmmoPack"))
        {
            Debug.Log("Player collided with ammo pack!");

            // Add ammo to the weapon using the PickupAmmo method
            AmmoPack ammoPack = other.GetComponent<AmmoPack>();
            if (ammoPack != null)
            {
                weapon.PickupAmmo(ammoPack.ammoAmount);
                Destroy(other.gameObject); // Destroy the ammo pack after pickup
            }
        }
    }
}
