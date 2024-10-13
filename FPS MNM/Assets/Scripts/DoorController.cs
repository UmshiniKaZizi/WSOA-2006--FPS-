using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Rotation Parameters")]
    [SerializeField]
    public Vector3 openRotation = new Vector3(-90, 0, 0);  // Rotation angle for the open state
    public float animationSpeed = 2f;  // Speed of the rotation animation

    [Header("Key Requirement")]
    public KeyItem requiredKey;  // Reference to the required key

    [Header("Door Type Settings")]
    [SerializeField] private bool isKeypadDoor = false;  // Is this door operated by a keypad?

    private Quaternion closedRotation;  // Store the initial rotation (closed state)
    private Quaternion openRotationQuaternion;  // Store the open state rotation
    private bool isOpen = false;  // Track if the door is open

    private void Start()
    {
        closedRotation = transform.localRotation;  // Store the initial rotation
        openRotationQuaternion = closedRotation * Quaternion.Euler(openRotation);  // Calculate the open rotation
    }

    public void ToggleDoor(KeyItem playerKey)
    {
        if (isKeypadDoor)
        {
            Debug.Log("This door requires a keypad to open.");
            return;  // Skip key logic if it's a keypad-operated door
        }

        // Check if the required key is present before opening
        if (requiredKey != null && (playerKey == null || playerKey != requiredKey))
        {
            Debug.Log("You need the correct key to open this door!");
            return;  // Prevent opening if the player doesn't have the correct key
        }

        ToggleDoorState();  // Toggle door state normally (non-keypad)
    }

    public void OpenDoorFromKeypad()
    {
        if (isKeypadDoor && !isOpen)
        {
            ToggleDoorState();  // Only open the door if it's a keypad door and not already open
        }
    }

    private void ToggleDoorState()
    {
        StopAllCoroutines();  // Stop any running animations

        // Set the target rotation based on the current state
        Quaternion targetRotation = isOpen ? closedRotation : openRotationQuaternion;

        StartCoroutine(AnimateDoorRotation(targetRotation));  // Start rotation animation
        isOpen = !isOpen;  // Toggle the state
    }

    private System.Collections.IEnumerator AnimateDoorRotation(Quaternion targetRotation)
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
