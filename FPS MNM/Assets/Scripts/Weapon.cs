using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage = 10f;
    public float Range = 100f;
    public float Force = 30f;
    public FirstPersonControls FirstPersonControls;
    public Camera playerCamera; // Add this line
    public ParticleSystem muzzleFlash;
    public GameObject BulletImpactEffect_1;
    public GameObject BulletImpactEffect_2;
    public Transform CanvasTransform;
   
    /*private void OnCollisionEnter(Collision collision)
    {
        Transform objColl = collision.transform;
        if (collision != null) 
        {
            Instantiate(BulletImpactPrefab, objColl);
        }
    }*/

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
            GameObject BulletImpact_1 = Instantiate(BulletImpactEffect_1, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject BulletImpact_2 = Instantiate(BulletImpactEffect_2, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(BulletImpact_1, 2f);
            Destroy(BulletImpact_2,2f);

           /* Rigidbody hitRigidbody = hit.transform.GetComponent<Rigidbody>();
            if (hitRigidbody != null)
            {
                Vector3 forceDirection = -hit.normal * Force;
                hitRigidbody.AddForce(forceDirection, ForceMode.Impulse);
            }*/
        }

        // Instantiate the UI Image at the determined impact point
        /*GameObject bulletHit = Instantiate(BulletImpactPrefab, hit.transform);
        bulletHit.transform.SetParent(CanvasTransform, false);

        // Convert the impact point to screen position
        Vector3 screenPoint = playerCamera.WorldToScreenPoint(impactPoint);
        RectTransform bulletHitRectTransform = bulletHit.GetComponent<RectTransform>();

        // Adjust the position in the canvas
        bulletHitRectTransform.anchoredPosition = new Vector2(screenPoint.x, screenPoint.y);
        bulletHitRectTransform.localScale = Vector3.one; // Ensure it maintains its scale

        // Activate the bullet hit image
        bulletHit.SetActive(true);

        Destroy(bulletHit, 2f);*/


    }

}
