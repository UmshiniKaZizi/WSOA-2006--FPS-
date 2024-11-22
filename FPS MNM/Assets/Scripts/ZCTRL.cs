using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCTRL : MonoBehaviour
{
    private Animator mc;
    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<Animator>();  
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.A))
        {
            mc.CrossFade("Jump", 1);
        } 
    }
}
