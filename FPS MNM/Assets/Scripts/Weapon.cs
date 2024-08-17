using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float Damage = 10f;
    public float Range = 100f;
    public FirstPersonControls FirstPersonControls;
    public Transform Camera;

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

        RaycastHit[] hits;
        Vector3 rayDirection = Camera.forward;
        hits = Physics.RaycastAll(Camera.position, rayDirection, Range);

        if (hits.Length == 0)
        {
            Debug.Log("No hits detected");
        }
        else
        {
            foreach (RaycastHit hit in hits)
            {
                Debug.Log("Hit: " + hit.transform.name);
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Debug.Log("Applying damage to: " + hit.transform.name);
                    enemy.enemydamage(Damage);
                }
            }
        }
    }

}
