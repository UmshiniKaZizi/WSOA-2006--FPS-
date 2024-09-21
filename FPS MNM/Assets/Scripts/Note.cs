using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public GameObject note;
    // Start is called before the first frame update
    void Start()
    {
        note.SetActive(false);
    }

    public void OnMouseOver()
    {
        note.SetActive(true);
    }

    public void OnMouseExit()
    {
        note.SetActive(false);
    }

}
