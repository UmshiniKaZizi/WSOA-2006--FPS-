using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public
        FirstPersonControls FirstPersonControls;
    public  GameObject player;
    private void Start()
    {
        FirstPersonControls = FindObjectOfType<FirstPersonControls>();
        if (FirstPersonControls == null)
        {
            Debug.LogError("FirstPersonControls script not found on any GameObject in the scene.");
        }
    }

   public void OnTriggerStay(Collider other)
{
    if (other.CompareTag("Player"))
    {
        Debug.Log("Player inside teleportation trigger");
        player = other.gameObject;
        if (FirstPersonControls.canTeleport == true)
        {
            other.transform.position = FirstPersonControls.TeleportLocation.position;
            Debug.Log("Player teleported to " + FirstPersonControls.TeleportLocation.position);
            FirstPersonControls.ResetTeleport();
        }
    }
}


}
