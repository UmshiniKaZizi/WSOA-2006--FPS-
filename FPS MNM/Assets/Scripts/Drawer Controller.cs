using System.Collections;
using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [Header("Drawer/Chest Settings")]
    [SerializeField] private bool isChest = false;  // Set to true if this object is a chest
    [SerializeField] private Transform lid;  // Assign the lid object for chest rotation
    [SerializeField] private Vector3 openPositionOffset = new Vector3(0.0f, 0.0f, 0.5f);
    [SerializeField] private Vector3 openRotation = new Vector3(-90f, 0f, 0f);  // Rotation for the chest lid
    [SerializeField] private float animationSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private Quaternion closedRotation;
    private Quaternion openRotationQuaternion;
    private bool isOpen = false;

    private void Start()
    {
        closedPosition = transform.localPosition;

        if (isChest && lid != null)
        {
            // Set closed and open rotations for the lid of the chest
            closedRotation = lid.localRotation;
            openRotationQuaternion = closedRotation * Quaternion.Euler(openRotation);
        }
        else
        {
            // For drawers, set the open position based on offset
            openPosition = closedPosition + openPositionOffset;
        }
    }

    public void ToggleDrawerOrChest()
    {
        StopAllCoroutines();

        if (isChest && lid != null)
        {
            // For chests, toggle the rotation of the lid
            Quaternion targetRotation = isOpen ? closedRotation : openRotationQuaternion;
            StartCoroutine(RotateLid(targetRotation));
        }
        else
        {
            // For drawers, toggle the position
            Vector3 targetPosition = isOpen ? closedPosition : openPosition;
            StartCoroutine(MoveDrawer(targetPosition));
        }

        isOpen = !isOpen;
    }

    private IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }

        transform.localPosition = targetPosition;
    }

    private IEnumerator RotateLid(Quaternion targetRotation)
    {
        while (Quaternion.Angle(lid.localRotation, targetRotation) > 0.01f)
        {
            lid.localRotation = Quaternion.RotateTowards(lid.localRotation, targetRotation, Time.deltaTime * animationSpeed * 100f);
            yield return null;
        }

        lid.localRotation = targetRotation;
    }
}
