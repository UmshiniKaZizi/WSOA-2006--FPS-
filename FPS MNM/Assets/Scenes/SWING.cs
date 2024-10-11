using UnityEngine;

public class SWING : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            animator.SetTrigger("SWING");
        }
    }
}
