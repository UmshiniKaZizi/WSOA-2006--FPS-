using UnityEngine;

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
    public Transform CanvasTransform;
   
   
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

        RaycastHit hit;
        Vector3 rayDirection = playerCamera.transform.forward;
        // Perform a single raycast instead of multiple hits
        bool hitDetected = Physics.Raycast(playerCamera.transform.position, rayDirection, out hit, Range);

        // Determine the point to use for the impact image
        Vector3 impactPoint = hitDetected ? hit.point : playerCamera.transform.position + rayDirection * Range;

        if (!hitDetected)
        {
            Debug.Log("No hits detected");
        }
        else
        {
            muzzleFlash.Play();
            Debug.Log("Hit: " + hit.transform.name);

            // Damage and force logic
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Applying damage to: " + hit.transform.name);
                enemy.enemydamage(Damage);
            }
            GameObject BulletImpact_1=Instantiate(BulletImpactEffect_1, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject BulletImpact_2 = Instantiate(BulletImpactEffect_1, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(BulletImpact_1, 2f);
            Destroy(BulletImpact_2,2f);

        }
    }

}
