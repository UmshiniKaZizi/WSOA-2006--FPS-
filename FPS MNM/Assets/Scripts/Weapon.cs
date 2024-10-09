using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public float Damage = 10f;
    public float Range = 100f;
    public float Force = 30f;
    public FirstPersonControls FirstPersonControls;
    public Camera playerCamera;
    public ParticleSystem muzzleFlash;
    public GameObject BulletImpactEffect_1;
    public GameObject BulletImpactEffect_2;

    public int maxBullets = 30; // Maximum bullets in a full magazine
    public int currentBullets; // Current bullets remaining in the magazine
    public TMP_Text ammoDisplay_TMP; // Reference to the TMP text element for displaying ammo

    private void Start()
    {
        // Initialize current bullets to max at the start
        currentBullets = maxBullets;

        // Update the ammo display at the beginning
        UpdateAmmoDisplay();
    }

    public void Shoot()
    {
        if (FirstPersonControls == null)
        {
            Debug.LogError("FirstPersonControls is not assigned!");
            return;
        }

        if (!FirstPersonControls.holdingGun)
        {
            Debug.LogError("No weapon equipped!");
            return;
        }

        if (currentBullets <= 0)
        {
            Debug.Log("No bullets left! Need to reload.");
            return;
        }

        muzzleFlash.Play();
        currentBullets--; // Decrease bullet count
        UpdateAmmoDisplay();

        RaycastHit hit;
        Vector3 rayDirection = playerCamera.transform.forward;
        bool hitDetected = Physics.Raycast(playerCamera.transform.position, rayDirection, out hit, Range);

        if (hitDetected)
        {
            Debug.Log("Hit: " + hit.transform.name);

            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Applying damage to: " + hit.transform.name);
                enemy.enemydamage(Damage);
            }

            GameObject BulletImpact_1 = Instantiate(BulletImpactEffect_1, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject BulletImpact_2 = Instantiate(BulletImpactEffect_2, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(BulletImpact_1, 2f);
            Destroy(BulletImpact_2, 2f);
        }
    }

    public void Reload()
    {
        Debug.Log("Reloading...");
        currentBullets = maxBullets; // Reset the bullet count to the max
        UpdateAmmoDisplay();
    }

    public void PickupAmmo(int amount)
    {
        currentBullets += amount;
        if (currentBullets > maxBullets)
        {
            currentBullets = maxBullets; // Cap at maximum bullets
        }
        UpdateAmmoDisplay();
    }

    private void UpdateAmmoDisplay()
    {
        if (ammoDisplay_TMP != null)
        {
            ammoDisplay_TMP.text = "Ammo: " + currentBullets + " / " + maxBullets;
        }
        else
        {
            Debug.LogWarning("Ammo display TMP text is not assigned!");
        }
    }
}
