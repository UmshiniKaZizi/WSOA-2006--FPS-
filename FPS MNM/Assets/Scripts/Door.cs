using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public Transform hinge; // Reference to the hinge point
    public float openAngle = 90f; // Angle the door should open
    public float openSpeed = 2f; // Speed of door opening
    private bool isOpen = false; // Track whether door is open

    private Quaternion closedRotation; // Store the closed rotation
    private Quaternion openRotation; // Store the open rotation

    void Start()
    {
        closedRotation = hinge.rotation; // Set initial rotation as closed
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0); // Calculate open rotation
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines(); // Stop previous animations
        StartCoroutine(AnimateDoor(isOpen ? openRotation : closedRotation));
    }

    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.01f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
    }
}
