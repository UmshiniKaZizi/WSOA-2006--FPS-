using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [Header("Rotation Parameters")]
    [SerializeField]
    public Vector3 openRotation = new Vector3(-90, 0, 0);  
    public float animationSpeed = 2f; 

    [Header("Key Requirement")]
    public KeyItem requiredKey;  

    [Header("Door Type Settings")]
    [SerializeField] private bool isKeypadDoor = false; 

    private Quaternion closedRotation;  
    private Quaternion openRotationQuaternion;  
    private bool isOpen = false;  

    private void Start()
    {
        closedRotation = transform.localRotation; 
        openRotationQuaternion = closedRotation * Quaternion.Euler(openRotation);
    }

    public void ToggleDoor(KeyItem playerKey)
    {
        // If it's a keypad door, inform the player
        if (isKeypadDoor)
        {
            Debug.Log("This door requires a keypad to open.");
            return;
        }

        // If the door requires a key and the player does not have the correct key
        if (requiredKey != null && (playerKey == null || playerKey != requiredKey))
        {
            Debug.Log("You need the correct key to open this door!");
            return;
        }

        // If no key is required and it is not a keypad door, open the door
        if (requiredKey == null && isKeypadDoor== false)
        {
            Debug.Log("The door is unlocked and can be opened.");
        }

        ToggleDoorState(); // Open or close the door
    }


    public void OpenDoorFromKeypad()
    {
        if (isKeypadDoor && !isOpen)
        {
            ToggleDoorState();
        }
    }

    private void ToggleDoorState()
    {
        StopAllCoroutines(); 

        Quaternion targetRotation = isOpen ? closedRotation : openRotationQuaternion;

        StartCoroutine(AnimateDoorRotation(targetRotation)); 
        isOpen = !isOpen;  
    }

    private IEnumerator AnimateDoorRotation(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.localRotation, targetRotation) > 0.01f)
        {
            // Smoothly rotate towards the target rotation
            transform.localRotation = Quaternion.RotateTowards(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * animationSpeed * 100f  // Adjust speed factor for smoother rotation
            );
            yield return null;  // Wait for the next frame
        }

        // Ensure the final rotation matches the target exactly
        transform.localRotation = targetRotation;
    }
}
