using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Projectile Settings")]
    public GameObject projectilePrefab;  
    public Transform firePoint;  
    public float shootInterval = 2f;  
    public float projectileSpeed = 20f; 

    private void Start()
    {
        InvokeRepeating(nameof(ShootProjectile), 1f, shootInterval);  
    }

    private void ShootProjectile()
    {
        
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

       
        Destroy(projectile, 1f);
    }
}
