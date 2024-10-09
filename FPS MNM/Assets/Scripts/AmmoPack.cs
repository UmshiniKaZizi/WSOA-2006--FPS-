using UnityEngine;

public class AmmoPack : MonoBehaviour
{
    public int ammoAmount = 10; // Amount of ammo this pack provides

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has collided with the ammo pack
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.PickupAmmo(ammoAmount); // Add ammo to the weapon
            Destroy(gameObject); // Remove the ammo pack from the scene
        }
    }
}
