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
        if (hinge == null)
        {
            hinge = transform;  // Fallback to the door's own Transform if not assigned
        }

        closedRotation = hinge.rotation;
        openRotation = Quaternion.Euler(closedRotation.eulerAngles.x, closedRotation.eulerAngles.y + openAngle, closedRotation.eulerAngles.z);

        Debug.Log("Initial Hinge Rotation: " + closedRotation.eulerAngles);
        Debug.Log("Target Open Rotation: " + openRotation.eulerAngles);
    }


    public void ToggleDoor()
    {
        isOpen = !isOpen;
        StopAllCoroutines();

        // Debug log to verify correct target rotation
        Debug.Log("Toggling Door. New State: " + (isOpen ? "Open" : "Closed"));
        Debug.Log("Target Rotation Should Be: " + (isOpen ? openRotation.eulerAngles : closedRotation.eulerAngles));

        StartCoroutine(AnimateDoor(isOpen ? openRotation : closedRotation));  // Ensure correct rotation is passed here
    }


    private IEnumerator AnimateDoor(Quaternion targetRotation)
    {
        Debug.Log("Starting Animation. Initial Hinge Rotation: " + hinge.rotation.eulerAngles);
        Debug.Log("Target Rotation: " + targetRotation.eulerAngles);

        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Slerp(hinge.rotation, targetRotation, Time.deltaTime * openSpeed);
            Debug.Log("Animating Door. Current Hinge Rotation: " + hinge.rotation.eulerAngles);
            yield return null;
        }

        // Ensure final rotation is set correctly
        hinge.rotation = targetRotation;
        Debug.Log("Door Animation Completed. Final Hinge Rotation: " + hinge.rotation.eulerAngles);
    }

}
