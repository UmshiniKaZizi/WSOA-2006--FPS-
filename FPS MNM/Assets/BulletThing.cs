using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletThing : MonoBehaviour
{
    public GameObject BulletImpactPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Transform objColl = collision.transform;
        if (collision != null)
        {
            Instantiate(BulletImpactPrefab, objColl);
        }
    }
}
