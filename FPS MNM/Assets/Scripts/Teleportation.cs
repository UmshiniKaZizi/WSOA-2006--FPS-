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

   


}
