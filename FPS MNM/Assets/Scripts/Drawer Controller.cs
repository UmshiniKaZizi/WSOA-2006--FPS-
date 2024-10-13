using System.Collections;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [Header("Drawer Movement Settings")]
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0.0f, 0.0f, 0.5f);  // Position offset when open
    [SerializeField] private float animationSpeed = 2f;  // Speed of the drawer animation

    private Vector3 closedPosition;  // The closed position of the drawer
    private Vector3 openPosition;    // The target position when the drawer is open
    private bool isOpen = false;     // Track if the drawer is open

    private void Start()
    {
        // Set the closed and open positions
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openPositionOffset;
    }

    public void ToggleDrawer()
    {
        // Stop any currently running animations
        StopAllCoroutines();

        // Start animating to the target position
        Vector3 targetPosition = isOpen ? closedPosition : openPosition;
        StartCoroutine(MoveDrawer(targetPosition));

        // Toggle the open state
        isOpen = !isOpen;
    }

    private IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            // Move smoothly towards the target position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }

        // Ensure the final position matches the target exactly
        transform.localPosition = targetPosition;
    }
}
