using UnityEngine;

public class SpikeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementDistance = 3f;  
    public float speed = 2f;  
    private Vector3 initialPosition;
    private bool movingIn = true;

    private void Start()
    {
        initialPosition = transform.position;  
    }

    private void Update()
    {
        MoveSpikes();
    }

    private void MoveSpikes()
    {
        if (movingIn)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + transform.forward * movementDistance, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition + transform.forward * movementDistance) < 0.1f)
            {
                movingIn = false;  
            }
        }
        else
        {
            
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                movingIn = true;  
            }
        }
    }
}
